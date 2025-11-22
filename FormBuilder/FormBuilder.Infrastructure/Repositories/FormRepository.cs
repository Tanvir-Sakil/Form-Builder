using FormBuilder.Domain.DTOs;
using FormBuilder.Domain.Entities;
using FormBuilder.Domain.Repositories;
using FormBuilder.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FormBuilder.Infrastructure.Repositories
{
    public class FormRepository : IFormRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public FormRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<int>SaveFormAsync (CreateFormDto dto,IDbTransaction dbTransaction =null )
        {
            var fieldsJson = JsonSerializer.Serialize(dto.Fields);

            if(dbTransaction !=null)
            {
                using var cmd  =((SqlConnection)dbTransaction.Connection).CreateCommand();
                cmd.Transaction = (SqlTransaction)dbTransaction;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_SaveForm";
                cmd.Parameters.AddWithValue("@Title", dto.Title);
                cmd.Parameters.AddWithValue("@Fields", fieldsJson ?? (object)DBNull.Value);

                var reader = await cmd.ExecuteReaderAsync();
                int createdId = 0;

                if(await reader.ReadAsync())
                {
                    createdId = reader.GetInt32(0);
                }
                reader.Close();
                return createdId;
            }
            else
            {
                using var conn = _applicationDbContext.CreateConnection();

                using var cmd = conn.CreateCommand();
                cmd.CommandType= CommandType.StoredProcedure;
                cmd.CommandText = "SP_SaveForm";
                cmd.Parameters.Add(new SqlParameter("@Title",dto.Title));
                cmd.Parameters.Add(new SqlParameter("@Fields",fieldsJson?? (object)DBNull.Value));

                await ((SqlConnection)conn).OpenAsync();

                using var reader = await ((SqlCommand)cmd).ExecuteReaderAsync();

                int createdId = 0;

                if(await reader.ReadAsync())
                {
                    createdId = reader.GetInt32(0);
                }
                reader.Close();
                return createdId;
            }
        }

        public async Task<(int totalCount,IEnumerable<FormListDto>items)> GetFormsPagedAsync(int start,int length ,string search)
        {
            var list = new List<FormListDto>();

            int totalCount = 0;

            using var conn = _applicationDbContext.CreateConnection();
            using var cmd = conn.CreateCommand();

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "SP_GetFormsPaged";
            cmd.Parameters.Add(new SqlParameter("@Start",start));
            cmd.Parameters.Add(new SqlParameter("@Length",length));
            cmd.Parameters.Add(new SqlParameter("@Search",string.IsNullOrEmpty(search)?(object)DBNull.Value : search));

            await ((SqlConnection)conn).OpenAsync();

            using var reader = await ((SqlCommand)cmd).ExecuteReaderAsync();

            if(await reader.ReadAsync())
            {
                totalCount = reader.GetInt32(0);
            }

            if(await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new FormListDto
                    {
                        FormId = reader.GetInt32(reader.GetOrdinal("FormId")),
                        Title = reader.GetString(reader.GetOrdinal("Title")), 
                    });

                }
            }
            return (totalCount, list);

        }

        public async  Task<FormDetails> GetFormDetailsAsync(int formId)
        {
            using var conn = _applicationDbContext.CreateConnection();
            using var cmd = conn.CreateCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetFormDetails";
            cmd.Parameters.Add(new SqlParameter("@FormId", formId));


            await ((SqlConnection)conn).OpenAsync();

            using var reader = await ((SqlCommand)cmd).ExecuteReaderAsync();

            FormDetails dto = null;

            if(await reader.ReadAsync())
            {
                dto = new FormDetails
                {
                    FormId = reader.GetInt32(reader.GetOrdinal("FormId")),
                    Title = reader.GetString(reader.GetOrdinal("Title"))
                };
            }
            if(dto!=null && await reader.NextResultAsync())
            {
                if (dto.Fields == null)
                    dto.Fields = new List<Form>();
                while (await reader.ReadAsync())
                {

                    dto.Fields.Add(new Form
                    {
                        FieldId = reader.GetInt32(reader.GetOrdinal("FieldId")),
                        Label = reader.GetString(reader.GetOrdinal("Label")),
                        IsRequired = reader.GetBoolean(reader.GetOrdinal("IsRequired")),
                        SelectedValue = reader.IsDBNull(reader.GetOrdinal("SelectedValue")) ? null : reader.GetString(reader.GetOrdinal("SelectedValue")),
                        OrderNo = reader.GetInt32(reader.GetOrdinal("OrderNo"))

                    });
                }
            }
            return dto;
        }
    }
}

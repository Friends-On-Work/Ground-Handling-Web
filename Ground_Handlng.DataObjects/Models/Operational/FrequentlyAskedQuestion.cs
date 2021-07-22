using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Others;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ground_Handlng.DataObjects.Models.Operational
{
    [Table("FrequentlyAskedQuestion")]
    public class FrequentlyAskedQuestion : AuditLog
    {
        public long Id { get; set; }
        public string Question { get; set; } 
        public string Answer { get; set; }
        public string ImageUrl { get; set; }
        public string Type { get; set; }

        ApplicationDbContext _context;

        public FrequentlyAskedQuestion(ApplicationDbContext context)
        {
            _context = context;
        }

        public FrequentlyAskedQuestion()
        {

        }

        public async Task<object> Save()
        {
            try
            {
                StartDate = DateTime.Now;
                EndDate = DateTime.MaxValue;
                CreationDate = DateTime.Now;
                RevisionDate = DateTime.MinValue;
                Status = RecordStatus.Active;
                _context.FrequentlyAskedQuestions.Add(this);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return new DatabaseOperationResponse
                    {
                        Status = OperationStatus.SUCCESS,
                        Message = "Record Sucessfully Created",
                    };
                }

                return new DatabaseOperationResponse
                {

                    Message = "Operation Was Not Sucessful",
                    Status = OperationStatus.Ok
                };

            }
            catch (Exception ex)
            {
                return new DatabaseOperationResponse
                {
                    ex = ex,
                    Message = "Error Occured While Creating Record",
                    Status = OperationStatus.ERROR
                };
            }
        }

        public async Task<object> Update()
        {
            try
            {
                this.RevisionDate = DateTime.Now;
                this.Status = RecordStatus.Active;
                _context.FrequentlyAskedQuestions.Attach(this);
                _context.Entry(this).State = EntityState.Modified;
                if (await _context.SaveChangesAsync() > 0)
                {
                    return new DatabaseOperationResponse
                    {
                        Status = OperationStatus.SUCCESS,
                        Message = "Record Sucessfully Updated",
                    };
                }
                else
                {
                    return new DatabaseOperationResponse
                    {
                        Message = "Operation Was Not Sucessful",
                        Status = OperationStatus.Ok
                    };
                }
            }
            catch (Exception ex)
            {
                return new DatabaseOperationResponse
                {
                    ex = ex,
                    Message = "Error Occured While Updating Record",
                    Status = OperationStatus.ERROR
                };
            }
        }

        public async Task<object> Delete()
        {
            try
            {
                this.Status = RecordStatus.Inactive;
                this.RevisionDate = DateTime.Now;
                EndDate = DateTime.Now;
                _context.FrequentlyAskedQuestions.Attach(this);
                _context.Entry(this).State = EntityState.Modified;
                if (await _context.SaveChangesAsync() > 0)
                {
                    return new DatabaseOperationResponse
                    {
                        Status = OperationStatus.SUCCESS,
                        Message = "Record Sucessfully Deleted",
                    };

                }
                else
                {
                    return new DatabaseOperationResponse
                    {
                        Message = "Operation Was Not Sucessful",
                        Status = OperationStatus.Ok
                    };
                }
            }
            catch (Exception ex)
            {
                return new DatabaseOperationResponse
                {
                    ex = ex,
                    Message = "Error Occured While Deleting Record",
                    Status = OperationStatus.ERROR
                };
            }
        }

        public async Task<List<object>> GetList()
        {
            try
            {
                var fdrMMRMasters = await Task.Run(() => _context.FrequentlyAskedQuestions.Where(con => con.Status == RecordStatus.Active).Cast<object>().ToList());
                return fdrMMRMasters;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<object> Refresh()
        {
            try
            {
                return await _context.FrequentlyAskedQuestions.Where(con => con.Id == Id).AsNoTracking().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Exist()
        {
            try
            {
                var fdrMMR = await _context.FrequentlyAskedQuestions.FirstOrDefaultAsync(con => con.Type == Type && con.Status == RecordStatus.Active);
                return fdrMMR == null ? false : true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<FrequentlyAskedQuestion> GetBook()
        {
            return await _context.FrequentlyAskedQuestions.FirstOrDefaultAsync(con => con.Type == Type && con.Status == RecordStatus.Active);
        }
    }
}

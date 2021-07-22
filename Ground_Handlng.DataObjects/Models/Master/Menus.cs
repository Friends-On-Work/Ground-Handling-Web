using Microsoft.EntityFrameworkCore;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Others;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ground_Handlng.DataObjects.Models.Master
{
    public class Menus : AuditLog
    {
        public Menus()
        {

        }
        [Key]
        public long MenuId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        [ForeignKey("ParentMenuId")]
        public long? ParentId { get; set; }
        public string Privilages { get; set; }

        public virtual Menus ParentMenuId { get; set; }
        [NotMapped]
        private ApplicationDbContext applicationDbContext;
        public Menus(ApplicationDbContext _applicationDbContext)
        {
            applicationDbContext = _applicationDbContext;
            applicationDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public async Task<List<Menus>> GetList()
        {
            return await applicationDbContext.Menus.ToListAsync();
        }
        public object Save()
        {
            try
            {
                this.StartDate = DateTime.Now;
                this.EndDate = DateTime.MaxValue;
                this.CreationDate = DateTime.Now;
                this.RevisionDate = DateTime.Now;
                this.Status = RecordStatus.Active;
                applicationDbContext.Menus.Add(this);
                if (!Exist())
                {
                    if (applicationDbContext.SaveChanges() > 0)
                    {

                        return new DatabaseOperationResponse
                        {
                            Status = OperationStatus.SUCCESS,
                            Message = "successfully inserted."
                        };

                    }
                    return new DatabaseOperationResponse
                    {
                        Message = "was not successfully inserted.",
                        Status = OperationStatus.Ok
                    };
                }
                else
                {
                    return new DatabaseOperationResponse
                    {

                        Message = "Already Exist", //Resources.RecordAlreadyExist,
                        Status = OperationStatus.NOT_OK
                    };
                }

            }
            catch (Exception ex)
            {
                return new DatabaseOperationResponse
                {
                    ex = ex,
                    Message = "Error occured while inserting the record",
                    Status = OperationStatus.ERROR,
                };
            }
        }

        public object Update()
        {
            try
            {
                this.RevisionDate = DateTime.Now;
                this.Status = RecordStatus.Active;
                applicationDbContext.Menus.Attach(this);
                applicationDbContext.Entry(this).State = EntityState.Modified;

                if (applicationDbContext.SaveChanges() > 0)
                {
                    return new DatabaseOperationResponse
                    {
                        Status =  OperationStatus.SUCCESS,
                        Message = "successfully updated."
                    };

                }
                else
                {
                    return new DatabaseOperationResponse
                    {
                        Message = "was not successfully updated.",
                        Status = OperationStatus.Ok
                    };
                }
            }
            catch (Exception ex)
            {
                return new DatabaseOperationResponse
                {
                    ex = ex,
                    Message = "Error occured while updating the",
                    Status =  OperationStatus.ERROR,
                };
            }
        }
        public object Delete()
        {
            try
            {
                this.Status = RecordStatus.Deleted;
                this.RevisionDate = DateTime.Now;
                this.EndDate = DateTime.Now;
                applicationDbContext.Menus.Attach(this);
                applicationDbContext.Entry(this).State = EntityState.Modified;

                if (applicationDbContext.SaveChanges() > 0)
                {
                    return new DatabaseOperationResponse
                    {
                        Status = OperationStatus.SUCCESS,
                        Message = "successfully deleted."
                    };

                }
                else
                {
                    return new DatabaseOperationResponse
                    {
                        Message = "was not successfully deleted.",
                        Status = OperationStatus.Ok
                    };
                }
            }
            catch (Exception ex)
            {
                return new DatabaseOperationResponse
                {
                    ex = ex,
                    Message = "Error occured while deleting the",
                    Status = OperationStatus.ERROR,
                };
            }
        }

        public bool Exist()
        {

            try
            {
                var menusFind = applicationDbContext.Menus.Where(c => c.Name == Name && c.ParentId == ParentId)?.FirstOrDefault();
                if (menusFind != null)
                {
                    if (menusFind.Status != RecordStatus.Active)
                    {
                        menusFind.Status = RecordStatus.Active;
                        applicationDbContext.Menus.Attach(menusFind);
                        applicationDbContext.Entry(menusFind).State = EntityState.Modified;
                        applicationDbContext.SaveChanges();
                    }
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

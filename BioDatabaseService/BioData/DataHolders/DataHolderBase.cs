using BioContracts;
using BioData.DataModels;
using BioData.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioData.DataHolders
{
  public class DataHolderBase<TValue>
  {
    public DataHolderBase(IProcessorLocator locator)
    {
      _locator     = locator;
      _dataContext = _locator.GetProcessor<BioSkyNetDataModel>();

      _convertor = new ProtoMessageConvertor();
    }

    public virtual TValue Add(TValue item)
    {     
      return item;    
    }

    public virtual bool Remove(TValue item) { return false; }

    public virtual TValue Update(TValue item)
    {
      return item;
    }
     
    public bool Save()
    {
      try
      {
        _dataContext.SaveChanges();
        return true;
      }
      catch (DbEntityValidationException e)
      {
          foreach (var eve in e.EntityValidationErrors)
          {
              Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                  eve.Entry.Entity.GetType().Name, eve.Entry.State);
              foreach (var ve in eve.ValidationErrors)
              {
                  Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                      ve.PropertyName, ve.ErrorMessage);
              }
          }
          throw;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }    

      return false;
    }

    protected readonly ProtoMessageConvertor _convertor  ;
    protected readonly BioSkyNetDataModel    _dataContext;

    protected readonly IProcessorLocator  _locator;
  }
}

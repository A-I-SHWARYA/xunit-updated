using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XunitAssessment.Models;
using XunitAssessment.Services.Interfaces;

namespace XunitAssessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AoColumnController : ControllerBase
    {
        private readonly AocolumnInterface aocolumninterface;

    
        public AoColumnController(AocolumnInterface aocolumninterface)
        {
          
            this.aocolumninterface = aocolumninterface;
        }


        //Add a column for the AOTable record created in the above  API
        [HttpPost]
        public async Task<IActionResult> AddColumn([FromBody] Aocolumn column)
        {
            try
            {
                if (column != null)
                {
                    column.Id = Guid.NewGuid();
                    var addedcolumn = await aocolumninterface.AddColumn(column);
                    if (addedcolumn != null)
                    {
                        return Ok(addedcolumn);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        // //Delete a record
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteColumn(Guid Id)
        {
            try
            {
                var deletedcolumn = await aocolumninterface.DeleteColumn(Id); 
                if(deletedcolumn != null)
                {
                    return Ok(deletedcolumn);
                }
                else
                {
                    return NotFound("Cannot delete because not found");

                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }


        //Get all records with DataType "int" and "uniqueidentifier" for a particular Table by passing TableName(AOTable) as parameter
        [HttpGet("{Name}")]
        public async Task<IActionResult> GetColumnsByType([FromRoute]string Name)
        {
            try
            {
                var Columns = await aocolumninterface.GetColumnsByType(Name);
                if (Columns != null)
                {
                    return Ok(Columns);
                }
                else { return NotFound("Columns not found"); }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using XunitAssessment.Models;
using XunitAssessment.Services.Interfaces;

namespace XunitAssessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AoTableController : ControllerBase
    {
        private readonly AotableInterface aotableInterface;

        public AoTableController(AotableInterface aotableInterface)
        {
            this.aotableInterface = aotableInterface;
        }



        //Add record
        [HttpPost]
        public async Task<IActionResult> AddTable([FromBody] Aotable atable)
        {
            try
            {
                if (atable != null)
                {
                    atable.Id = Guid.NewGuid();
                    var newrecord = await aotableInterface.AddTable(atable);
                    if (newrecord != null)
                    {
                        return Ok(newrecord);
                    }
                    else
                    {
                        return BadRequest("Table Cannot be null");
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


        //Edit a record
        [HttpPut("{Id}")]
        public async Task<IActionResult> EditTable([FromRoute]Guid Id, [FromBody]Aotable aotable)
        {
         
            try
            {
                if (aotable != null)
                {
                    var editrecord = await aotableInterface.EditTable(Id, aotable);
                    if (editrecord != null)
                    {
                        return Ok(editrecord);
                    }
                    else
                    {
                        return NotFound("Record cannot be editted!");
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




        //Get all records of Type "coverage" and "form". This Type needs to be a parameter.

        [HttpGet("{Type}")]
        public async Task<IActionResult> GetByType(string Type)
        {
            try
            {
                var formorcoverage = await aotableInterface.GetByType(Type);
                if (formorcoverage != null)
                {
                    return Ok(formorcoverage);
                }
                else
                {
                    return NotFound("Tables not found");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Get all records which has a particular word in the Name. The word needs to be a parameter.

        [HttpGet("{Name}/gettabledata")]
        public async Task<IActionResult> CheckName(string Name)
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    return BadRequest("The table name cannot be empty.");
                }

                var namerecord = await aotableInterface.CheckName(Name);
                if (namerecord != null)
                {
                    return Ok(namerecord);
                }
                else { return NotFound("The record with the particular name not found"); }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        




    }
}

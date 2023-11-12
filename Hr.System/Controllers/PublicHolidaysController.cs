using Hr.Application.DTOs;
using Hr.Application.Services.implementation;
using Hr.Application.Services.Interfaces;
using Hr.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hr.System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicHolidaysController : ControllerBase
    {
        IPublicHolidaysService publicHolidaysService;
        public PublicHolidaysController(IPublicHolidaysService publicHolidaysService)
        {
            this.publicHolidaysService = publicHolidaysService;
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            try
            {
                var publicHolidays = publicHolidaysService.GetAllPublicHolidays();
                if (publicHolidays != null)
                {
                    var publicHolidaysDTOs = publicHolidays.Select(x => new PublicHolidaysDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Date = x.Day.ToString("yyyy-MM-dd")
                    }).ToList();


                    return Ok(publicHolidaysDTOs);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            try
            {
                PublicHolidays publicHoliday = publicHolidaysService.GetPublicHolidayId(id);
                if (publicHoliday != null)
                {
                    var publicHolidayDTO = new PublicHolidaysDTO
                    {
                        Id = publicHoliday.Id,
                        Name = publicHoliday.Name,
                        Date = publicHoliday.Day.ToString("yyyy-MM-dd")
                    };
                    return Ok(publicHolidayDTO);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult Create(PublicHolidaysDTO publicHolidayDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (!publicHolidaysService.CheckPublicHolidaysExists(publicHolidayDTO))
                {
                    DateTime date = DateTime.Now;
                    DateTime publicHolidayDate= DateTime.Parse(publicHolidayDTO.Date);
                    if (publicHolidayDate> date)
                    {
                        var publicHoliday = new PublicHolidays()
                        {
                            Id = publicHolidayDTO.Id,
                            Name = publicHolidayDTO.Name,
                            Day = publicHolidayDate,
                        };
                        publicHolidaysService.Create(publicHoliday);
                        return Ok(new
                        {
                            message= "Public Holiday created successfully"
                        });
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            message = "Date should be greater than today"
                        });
                    }
                  
                }
                return BadRequest(new
                {
                    message= "The Public Holiday already exists"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, PublicHolidaysDTO publicHolidayDTO)
        {
            try
            {
                var existingPublicHoliday = publicHolidaysService.GetPublicHolidayId(id);
                if (existingPublicHoliday == null)
                {
                    return NotFound();
                }
                if (publicHolidayDTO == null || !ModelState.IsValid)
                {
                    return BadRequest(publicHolidayDTO);
                }
                DateTime date = DateTime.Now;
                DateTime publicHolidayDate = DateTime.Parse(publicHolidayDTO.Date);
                if( publicHolidayDate < date)
                {
                    return BadRequest(new
                    {
                        message = "Date should be greater than today"
                    });
                }
                if (!publicHolidaysService.GetAllPublicHolidays().Any(x => x.Name.ToLower() == publicHolidayDTO.Name.ToLower()  && x.Id != id))
                {
                    existingPublicHoliday.Id = id;
                    existingPublicHoliday.Name = publicHolidayDTO.Name;
                    existingPublicHoliday.Day = publicHolidayDate;
                    publicHolidaysService.Update(existingPublicHoliday);
                    return Ok(publicHolidayDTO);
                }
                return BadRequest(publicHolidayDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var publicHoliday = publicHolidaysService.GetPublicHolidayId(id);
                if (publicHoliday != null)
                {
                    publicHolidaysService.Remove(publicHoliday);
                    return NoContent();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }
        }

    }
}

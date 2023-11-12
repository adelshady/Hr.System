using Hr.Application.Common.Enums;
using Hr.Application.DTOs;
using Hr.Application.DTOs.Employee;
using Hr.Application.Services.implementation;
using Hr.Application.Services.Interfaces;
using Hr.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;
using System.Diagnostics.Metrics;




namespace Hr.System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralSettingsController : ControllerBase
    {
        IWeekendService weekendService;
        IGeneralSettingsService generalSettingsService;
        IEmployeeServices employeeServices;
        public GeneralSettingsController(IWeekendService weekendService, IGeneralSettingsService generalSettingsService, IEmployeeServices employeeServices)
        {
            this.weekendService = weekendService;
            this.generalSettingsService = generalSettingsService;
            this.employeeServices = employeeServices;
        }
        #region all emps

        [HttpGet]
        //Display CheckBox for Weekends
        public ActionResult Create()
        {
            try
            {
                var general = generalSettingsService.GetGeneralSettingForAll();

                List<string> weekDays = weekendService.Days();

                var weekendDTO = new WeekendDTO();
                if (general == null)
                {
                    weekendDTO = new WeekendDTO
                    {
                        Weekends = weekDays.Select(day => new WeekendCheckDTO
                        {
                            displayValue = day,
                            isSelected = false,
                        }).ToList()
                    };
                }
                else
                {
                    var weekdaysDb = weekendService.GetAllWeekends().Where(x => x.GeneralSettings.EmployeeId == null);

                    weekendDTO = new WeekendDTO
                    {
                        OvertimeHour = general.OvertimeHour,
                        DiscountHour = general.DiscountHour,
                        Id = general.Id,
                        empid = general.EmployeeId,
                        Weekends = weekDays.Select(day => new WeekendCheckDTO
                        {
                            displayValue = day,
                            isSelected = weekdaysDb.Any(x => x.Name == day)
                        }).ToList()
                    };
                }
                IEnumerable<GetAllEmployeeDto> employeeDTOs = employeeServices.GetAllEmployee();
                IEnumerable<SelectListItem> employeeSelectList = employeeDTOs.Select(dto => new SelectListItem
                {
                    Value = dto.ID.ToString(),
                    Text = dto.FirstName
                }).ToList();
                weekendDTO.EmployeeList = employeeSelectList;


                return Ok(weekendDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        #endregion

        #region custom 

        [HttpGet("{empid}")]
        //when relation between emp & his settings (custom Settings)
        public ActionResult GetById(int empid)
        {
            try
            {
                List<string> weekDays = weekendService.Days();
                var weekdaysDb = weekendService.GetAllWeekends().Where(x => x.GeneralSettings.EmployeeId == empid);

                var Customsettings = generalSettingsService.GetGeneralSettingId(empid);

                if (Customsettings == null)
                {
                    var Deafultweekend = new WeekendDTO
                    {
                        empid = empid,
                        Weekends = weekDays.Select(day => new WeekendCheckDTO
                        {
                            displayValue = day,
                            isSelected = false
                        }).ToList(),
                        EmployeeList = employeeServices.GetAllEmployeeForAttendance().Select(x => new SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = x.Name
                        }).ToList()

                    };
                    return Ok(Deafultweekend);

                }
                else
                {
                    var Weekends = weekendService.GetById(Customsettings.Id);

                    var DTO = new WeekendDTO
                    {
                        Id = Customsettings.Id,
                        OvertimeHour = Customsettings.OvertimeHour,
                        DiscountHour = Customsettings.DiscountHour,
                        empid = Customsettings.EmployeeId,
                        Weekends = weekDays.Select(day => new WeekendCheckDTO
                        {
                            displayValue = day,
                            isSelected = weekdaysDb.Any(x => x.Name == day)
                        }).ToList(),
                        EmployeeList = employeeServices.GetAllEmployeeForAttendance().Select(x => new SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = x.Name
                        }).ToList()

                    };
                    return Ok(DTO);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult UpdateGeneralSettings(WeekendDTO updatedSettings)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var CustomSettingsExisted = generalSettingsService.CheckGeneralSettingsExists(updatedSettings.empid);

                    if (CustomSettingsExisted == false)
                    {
                        if (updatedSettings.Weekends != null
                            && updatedSettings.DiscountHour != null
                            && updatedSettings.OvertimeHour != null
                            && updatedSettings.empid == 0
                            && updatedSettings.Id != null)
                        {

                            var employeeSetting = generalSettingsService.GetGeneralSettingByID(updatedSettings.Id);

                            var state = weekendService.Update(updatedSettings, employeeSetting.Id);
                            if (state == false)
                            {
                                return BadRequest(new { error = "Invalid request data." });
                            }
                            else
                            {
                                employeeSetting.OvertimeHour = updatedSettings.OvertimeHour;
                                employeeSetting.DiscountHour = updatedSettings.DiscountHour;
                                generalSettingsService.Update(employeeSetting);
                                
                                return Ok(new { message = "Updated General Settings successfully." });

                            }

                        }
                         
                      
                    }
                }
                else
                {
                    return BadRequest(updatedSettings);
                }

                var employeeSettings = generalSettingsService.GetGeneralSettingId(updatedSettings.empid.Value);

                var states = weekendService.Update(updatedSettings, employeeSettings.Id);
                if (states == false)
                {
                    return BadRequest(new { error = "Invalid request data." });
                }
                else
                {
                    employeeSettings.OvertimeHour = updatedSettings.OvertimeHour;
                    employeeSettings.DiscountHour = updatedSettings.DiscountHour;
                    generalSettingsService.Update(employeeSettings);
                    return Ok(updatedSettings);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult CreateSettings(WeekendDTO updatedWeekends)
        {
            try
            {
               
                int Counter = 0;
                foreach (var item in updatedWeekends.Weekends)
                {
                    if (!item.isSelected)
                    {
                        Counter++;
                    }
                }

                if (Counter == 7)
                {
                    return BadRequest("please select day!");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new { updatedWeekends });
                }

                var generalExists = generalSettingsService.CheckGeneralSettingsExists(updatedWeekends.empid);

                if (generalExists)
                {
                    return BadRequest(new { error = "Custom settings Already Exists!" });
                }

                if (updatedWeekends.empid == 0)
                {
                    updatedWeekends.empid = null;
                    var existsNull = generalSettingsService.GetGeneralSettingForAll();
                    if (existsNull != null)
                    {
                        return BadRequest(new { error = "General Settings Already Exists!" });
                    }
                }

                var general = new GeneralSettings
                {
                    OvertimeHour = updatedWeekends.OvertimeHour,
                    DiscountHour = updatedWeekends.DiscountHour,
                    EmployeeId = updatedWeekends.empid
                };

                generalSettingsService.Create(general);

                var selectedWeekends = updatedWeekends.Weekends.Where(x => x.isSelected).Select(x => x.displayValue).ToList();
                var created = new List<WeekendDTO>();
                foreach (var selectedDay in selectedWeekends)
                {
                    var weekend = new Weekend
                    {
                        Name = selectedDay,
                        GeneralSettingsId = general.Id
                    };
                    if (weekendService.CheckPublicHolidaysExists(weekend))
                    {
                        return BadRequest($"the day {weekend.Name} already selected before!");
                    }
                    weekendService.Create(weekend);
                    created.Add(updatedWeekends);
                }

                return Ok(created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public ActionResult Remove(int id)
        {
            try
            {
                var remove = generalSettingsService.GetGeneralSettingByID(id);
                if (remove == null)
                {
                    return NotFound(new { error = "Not Found" });
                }
                var weekendExited = weekendService.GetById(remove.Id);
                if (weekendExited.Count() != 0)
                {
                    foreach (var item in weekendExited)
                    {
                        weekendService.Delete(item);

                    }
                }
                generalSettingsService.Remove(remove);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred", message = ex.Message });
            }

        }

        #endregion
    }
}






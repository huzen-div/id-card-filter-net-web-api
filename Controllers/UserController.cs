using AutoMapper;
using BroadcastAPI.Entities.Users;
using BroadcastAPI.Helper;
using BroadcastAPI.Models;
using BroadcastAPI.Models.Users;
using BroadcastAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BroadcastAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private SqlConnection _dbCon;
        private string _connectionString;
        public UserController(ILogger<UserController> logger, IOptions<ConnectionStringList> connectionStrings
        , IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _logger = logger;
            _connectionString = connectionStrings.Value.PrimaryDatabase;
            _dbCon = new SqlConnection(_connectionString);
        }
        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            string methodName = "GetAll";
            try
            {
                _logger.LogInformation($"Start {methodName}");

                ConnectionHandle.openConnection(_dbCon);
                UserDetailService serv = new(_logger);
                List<Users> lstData = serv.GetAll(_dbCon);
                return Ok(lstData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} error: " + ConvertUtil.obj2string(ex));
                return BadRequest(ex.Message);
            }
            finally
            {
                ConnectionHandle.closeConnection(_dbCon);
            }
        }
        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult GetByFilter(UserFilterRequest model)
        {
            string methodName = "GetByFilter";
            try
            {
                _logger.LogInformation($"Start {methodName}");

                ConnectionHandle.openConnection(_dbCon);
                UserDetailService serv = new(_logger);
                List<Users> lstData = serv.GetByFilter(_dbCon, model);
                return Ok(lstData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} error: " + ConvertUtil.obj2string(ex));
                return BadRequest(ex.Message);
            }
            finally
            {
                ConnectionHandle.closeConnection(_dbCon);
            }
        }
        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Create(UserCreateRequest model)
        {
            string methodName = "Create";
            try
            {
                _logger.LogInformation($"Start {methodName}");

                ConnectionHandle.openConnection(_dbCon);
                UserDetailService serv = new(_logger);
                int affectedRows = serv.Create(_dbCon, model);
                return Ok(new { message = "ทำรายการสำเร็จ"});
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} error: " + ConvertUtil.obj2string(ex));
                return BadRequest(ex.Message);
            }
            finally
            {
                ConnectionHandle.closeConnection(_dbCon);
            }
        }
        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Update(UserCreateRequest model)
        {
            string methodName = "Create";
            try
            {
                _logger.LogInformation($"Start {methodName}");

                ConnectionHandle.openConnection(_dbCon);
                UserDetailService serv = new(_logger);
                int affectedRows = serv.Update(_dbCon, model);
                return Ok(new { message = "ทำรายการสำเร็จ" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} error: " + ConvertUtil.obj2string(ex));
                return BadRequest(ex.Message);
            }
            finally
            {
                ConnectionHandle.closeConnection(_dbCon);
            }
        }
    }
}

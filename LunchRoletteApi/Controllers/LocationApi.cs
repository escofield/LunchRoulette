/*
 * Lunch RouletteAPI
 *
 * A slack API to generate random lunch locations using fluentMigrator.
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using IO.Swagger.Models;
using LunchRoletteApi.Repositories;

namespace IO.Swagger.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    public class LocationApiController : Controller
    { 

        /// <summary>
        /// Locations of places to eat
        /// </summary>
        /// <remarks>Location of places to eat. </remarks>
        /// <response code="200">An array of locations</response>
        /// <response code="0">Unexpected error</response>
        [HttpGet]
        [Route("/Location")]
        [ProducesResponseType(typeof(List<Location>), 200)]
        public virtual IActionResult LocationGet()
        {
            var lr = new LocationRepository();

            return Json(lr.GetLocations());
        }

        /// <summary>
        /// Random Lunch Location
        /// </summary>
        ///  /// <remarks>A random lunch location. </remarks>
        /// <response code="200">One location</response>
        /// <response code="0">Unexpected error</response>
        [HttpGet]
        [Route("/Lunch")]
        [ProducesResponseType(typeof(string), 200)]
        public virtual IActionResult LunchGet()
        {
            var lr = new LocationRepository();

            return Json(lr.GetLunch().DisplayName);
        }



    }
}

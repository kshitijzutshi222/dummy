#region Header
// © 2019 Koninklijke Philips N.V.  All rights reserved.
// Reproduction or transmission in whole or in part, in any form or by any means, 
// electronic, mechanical or otherwise, is prohibited without the prior  written consent of 
// the owner.
#endregion

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Philips.iX.MicroserviceName.DataModel;

namespace Philips.iX.MicroserivceName.Api
{
	[ApiController]
	[Route("api/[controller]")]
	public class ActorsController : Controller
	{

		#region Non-Public Data Members
		private readonly ILogger<ActorsController> _logger;
		private readonly ActorService _actorService;

		#endregion

		#region Non-Public Properties/Methods
		#endregion

		#region Public Properties/Methods


		public ActorsController(ILogger<ActorsController> logger, ActorService actorService)
		{
			_logger = logger;
			_actorService = actorService;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesDefaultResponseType]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public IActionResult AddActor(Actor actor)
		{
			_actorService.Add(actor);
			_logger.LogInformation(@"Added Actor: {0} {1}", actor.FirstName, actor.LastName);
			return Ok();
		}


		[HttpGet]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesDefaultResponseType]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public IActionResult GetActors([FromQuery] string firstName)
		{
			List<Actor> actor = _actorService.GetActorWithFirstName(firstName);
			return Ok(actor);
		}
		#endregion
	}
}

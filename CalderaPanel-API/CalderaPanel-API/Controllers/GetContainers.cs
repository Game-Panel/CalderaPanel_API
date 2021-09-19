using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Docker.DotNet;
using Docker.DotNet.Models;
using System.Threading.Tasks;

namespace CalderaPanel_API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GetContainersController : ControllerBase
	{
		[HttpGet]
		public async Task<IEnumerable<ContainerListResponse>> GetAsync(int limit)
		{
			DockerClient client = new DockerClientConfiguration(
			new Uri("npipe://./pipe/docker_engine"))
			 .CreateClient();

			IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { Limit = limit });
			return containers;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace CalderaPanel_API
{
    class Program
    {
        static async Task Main(string[] args)
        {
			DockerClient client = new DockerClientConfiguration(
				new Uri("npipe://./pipe/docker_engine"))
				 .CreateClient();

			IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
			new ContainersListParameters()
			{
				Limit = 10,
			});

			for(int i = 0; i < containers.Count; i++)
			{
				Console.WriteLine($"{containers[i].ImageID} - {containers[i].Image} - {containers[i].State}");
			}

			Console.ReadKey();
        }
    }
}

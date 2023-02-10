#region Header
// © 2020 Koninklijke Philips N.V.  All rights reserved.
// Reproduction or transmission in whole or in part, in any form or by any means, 
// electronic, mechanical or otherwise, is prohibited without the prior  written consent of 
// the owner.
// Author:      
// Date:        
#endregion

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Philips.iX.MicroserviceName.DataModel;

namespace Philips.iX.MicroserviceName.Client
{
	public class ActorsRestClient : IDisposable
	{
		private bool _isDisposed = false;
		private readonly object _serviceLock = new object();
		private readonly HttpClient _httpClient;

		public ActorsRestClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task AddActor(Actor actor)
		{
			string body = JsonConvert.SerializeObject(actor);
			byte[] formattedBody = System.Text.Encoding.UTF8.GetBytes(body);
			using ByteArrayContent content = new ByteArrayContent(formattedBody);
			content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
			Uri uri = new Uri($"api/actors", UriKind.Relative);
			HttpResponseMessage response = await _httpClient.PostAsync(uri, content).ConfigureAwait(false);
			_ = response.EnsureSuccessStatusCode();
		}


		public async Task<List<Actor>> GetActorsWithFirstName(string firstName)
		{
			Uri uri = new Uri($"api/actors?firstName={firstName}", UriKind.Relative);
			HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri).ConfigureAwait(false);
			_ = httpResponseMessage.EnsureSuccessStatusCode();
			string result = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
			return JsonConvert.DeserializeObject<List<Actor>>(result);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool isCalledByUser)
		{
			lock (_serviceLock)
			{
				if (_isDisposed)
				{
					return;
				}

				if (isCalledByUser)
				{
					_httpClient.Dispose();
				}

				_isDisposed = true;
			}
		}
	}
}

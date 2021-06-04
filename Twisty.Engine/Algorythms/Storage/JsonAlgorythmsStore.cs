using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Twisty.Engine.Algorythms.Storage
{
	public class JsonAlgorythmsStore
	{
		private readonly string m_StorePath;
		public JsonAlgorythmsStore(string storePath)
		{
			m_StorePath = storePath;
		}

		public AlgorythmsLibrary Read(string coreId)
		{
			// Generate full path.
			string path = GetPathForCore(coreId);

			// read JSON directly from a file
			using StreamReader file = File.OpenText(path);
			using JsonTextReader reader = new(file);

			JObject o = (JObject)JToken.ReadFrom(reader);

			return o.ToObject<AlgorythmsLibrary>();
		}

		public void Write(string coreId, AlgorythmsLibrary library)
		{
			// Generate full path.
			string path = GetPathForCore(coreId);

			var o = JToken.FromObject(library);

			using StreamWriter file = File.CreateText(path);
			using JsonTextWriter writer = new(file);
			o.WriteTo(writer);
		}

		// TODO : implement path traversal sanitizing.
		private string GetPathForCore(string coreId)
			=> Path.Combine(m_StorePath, coreId + ".json");
	}
}

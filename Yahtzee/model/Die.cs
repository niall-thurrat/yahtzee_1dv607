using System;
using Newtonsoft.Json;

namespace Yahtzee.model
{
	[Serializable]
	[JsonObject(MemberSerialization.OptIn)]
	class Die
	{
		private static System.Random g_random = new System.Random();

		public Die()
		{
			Value = 0;
            IsHeld = false;
		}

		[JsonProperty]
		public int Value { get; private set; }


		[JsonProperty]
		public bool IsHeld { get; set; }

		public void Roll()
		{
				Value = g_random.Next(1, 7);
		}
	}
}
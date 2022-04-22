using DOTA2RandomHeroPicker.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTA2RandomHeroPicker.Services {
	public static class RandomCalculator {
		private class HeroWeight {
			public Hero Hero { get; set; }
			public double Weight { get; set; }
			public HeroWeight(Hero hero, double weight) {
				Hero = hero;
				Weight = weight;
			}
			public override string ToString() {
				return $"{Hero.Name} - {Weight}";
			}
		}

		public static Hero Calculate2(params string[] lastPicks) {
			if(LocalData.Filter.Amount <= 0 || LocalData.Heroes == null || LocalData.Heroes.Count == 0) {
				return null;
			}
			FilterData filter = LocalData.Filter;
			List<HeroWeight> list = new();
			foreach(Hero item in LocalData.Heroes) {
				double heroTypeWeight = item.HeroType switch {
					HeroType.Strength => filter.GetStrengthWeight(),
					HeroType.Agility => filter.GetAgilityWeight(),
					HeroType.Intelligence => filter.GetIntelligenceWeight(),
					_ => 0,
				} / 100d;
				double heroRoleWeight = item.HeroRole switch {
					HeroRole.Core => filter.GetCoreWeight(),
					HeroRole.Support => filter.GetSupportWeight(),
					HeroRole.Both => 100,
					_ => 0,
				} / 100d;
				double difficultyWeight = item.HeroDifficulty switch {
					HeroDifficulty.One => filter.GetDifficultyOneWeight(),
					HeroDifficulty.Two => filter.GetDifficultyTwoWeight(),
					HeroDifficulty.Three => filter.GetDifficultyThreeWeight(),
					_ => 0,
				} / 100d;
				double attackTypeWeight = item.AttackType switch {
					AttackType.Ranged => filter.GetRangedWeight(),
					AttackType.Melee => filter.GetMeleeWeight(),
					_ => 0,
				} / 100d;
				double heroWeight = item.GetWeight() / 100d;

				double weight = heroTypeWeight * heroRoleWeight * difficultyWeight * attackTypeWeight * heroWeight;
				list.Add(new HeroWeight(item, weight));
			}

			list.RemoveAll(l => l.Weight == 0);
			var tmp = list.ToList();
			foreach(string name in lastPicks) {
				tmp.RemoveAll(l => l.Hero.Name == name);
			}
			if(tmp.Any()) {
				list = tmp;
			}

			for(int i = 1; i < list.Count; i++) {
				HeroWeight item = list[i];
				HeroWeight last = list[i - 1];
				item.Weight += last.Weight;
			}

			if(!list.Any()) {
				return null;
			}

			double random = new Random().NextDouble() * list.Last().Weight;
			foreach(HeroWeight item in list) {
				if(item.Weight > random) {
					AttributesSamples.Add(item.Hero);
					return item.Hero;
				}
			}
			return null;
		}

		public static List<Hero> AttributesSamples = new();

		public static void DebugSamplesAttributes() {
			double count = AttributesSamples.Count;
			double count_both = AttributesSamples.Count(h => h.HeroRole != HeroRole.Both);
			Debug.WriteLine("Role:");
			Debug.WriteLine("Core-> " + (AttributesSamples.Count(h => h.HeroRole == HeroRole.Core) / count_both));
			Debug.WriteLine("Support-> " + (AttributesSamples.Count(h => h.HeroRole == HeroRole.Support) / count_both));
			Debug.WriteLine("Attack Type:");
			Debug.WriteLine("Ranged-> " + (AttributesSamples.Count(h => h.AttackType == AttackType.Ranged) / count));
			Debug.WriteLine("Melee-> " + (AttributesSamples.Count(h => h.AttackType == AttackType.Melee) / count));
			Debug.WriteLine("Difficulty:");
			Debug.WriteLine("One-> " + (AttributesSamples.Count(h => h.HeroDifficulty == HeroDifficulty.One) / count));
			Debug.WriteLine("Two-> " + (AttributesSamples.Count(h => h.HeroDifficulty == HeroDifficulty.Two) / count));
			Debug.WriteLine("Three-> " + (AttributesSamples.Count(h => h.HeroDifficulty == HeroDifficulty.Three) / count));
			Debug.WriteLine("Type:");
			Debug.WriteLine("Strength-> " + (AttributesSamples.Count(h => h.HeroType == HeroType.Strength) / count));
			Debug.WriteLine("Agility-> " + (AttributesSamples.Count(h => h.HeroType == HeroType.Agility) / count));
			Debug.WriteLine("Intelligence-> " + (AttributesSamples.Count(h => h.HeroType == HeroType.Intelligence) / count));
		}

		public static void DebugSamplesHeroes() {
			//0.00813
			foreach(Hero hero in LocalData.Heroes) {
				Debug.WriteLine("-------------------------------------------");
				Debug.WriteLine($"{hero.GetFixedHeroName()}\n" +
					$"{Math.Round(AttributesSamples.Count(a => a.Name == hero.Name) / (double)AttributesSamples.Count, 6)}\n{Math.Round(1 / (double)123, 6)}");
			}
		}
	}
}

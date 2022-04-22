using Microsoft.UI.Xaml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace DOTA2RandomHeroPicker.Data {
	public static class LocalData {
		public const string HEROES_DATA_FILE_NAME = "Data.json";
		public const string FILTER_DATA_FILE_NAME = "Filter.json";
		public static StorageFolder LocalFolder => ApplicationData.Current.LocalFolder;
		public static StorageFile HeroesDataFile { get; private set; }
		public static StorageFile FilterDataFile { get; private set; }

		public static List<Hero> Heroes { get; private set; } = new List<Hero>();
		public static FilterData Filter { get; private set; } = new FilterData();

		public static async Task Initialize() {
			Debug.WriteLine("Initializing Local");
			Debug.WriteLine(LocalFolder.Path);
			HeroesDataFile = await LocalFolder.CreateFileAsync(HEROES_DATA_FILE_NAME, CreationCollisionOption.OpenIfExists);
			FilterDataFile = await LocalFolder.CreateFileAsync(FILTER_DATA_FILE_NAME, CreationCollisionOption.OpenIfExists);
			await LoadHeroes();
			await LoadFilters();
		}

		public static async Task SaveHeroes() {
			await FileIO.WriteTextAsync(HeroesDataFile, JsonConvert.SerializeObject(Heroes, Formatting.Indented));
		}

		public static async Task LoadHeroes() {
			string json = await FileIO.ReadTextAsync(HeroesDataFile);
			Heroes = JsonConvert.DeserializeObject<List<Hero>>(json);
			if(Heroes == null) {
				ResetHeroes();
				await SaveHeroes();
			}
		}

		public static async Task SaveFilters() {
			await FileIO.WriteTextAsync(FilterDataFile, JsonConvert.SerializeObject(Filter, Formatting.Indented));
		}

		public static async Task LoadFilters() {
			string json = await FileIO.ReadTextAsync(FilterDataFile);
			Filter = JsonConvert.DeserializeObject<FilterData>(json);
			if(Filter == null) {
				ResetFilters();
				await SaveFilters();
			}
		}

		public static void ResetFilters() {
			if(Filter == null) {
				Filter = new FilterData();
			}
			Filter.StrengthWeight = 100;
			Filter.StrengthEnabled = true;
			Filter.AgilityWeight = 100;
			Filter.AgilityEnabled = true;
			Filter.IntelligenceWeight = 100;
			Filter.IntelligenceEnabled = true;
			Filter.DifficultyOneWeight = 100;
			Filter.DifficultyOneEnabled = true;
			Filter.DifficultyTwoWeight = 100;
			Filter.DifficultyTwoEnabled = true;
			Filter.DifficultyThreeWeight = 100;
			Filter.DifficultyThreeEnabled = true;
			Filter.CoreWeight = 100;
			Filter.CoreEnabled = true;
			Filter.SupportWeight = 100;
			Filter.SupportEnabled = true;
			Filter.RangedWeight = 100;
			Filter.MeleeEnabled = true;
			Filter.MeleeWeight = 100;
			Filter.RangedEnabled = true;
			Filter.Amount = 1;
		}

		public static void ResetWeight() {
			foreach(Hero hero in Heroes) {
				hero.Weight = 100;
				hero.IsEnabled = true;
			}
		}

		public static void ResetHeroes() {
			if(Heroes == null) {
				Heroes = new List<Hero>();
			}
			Heroes.Clear();
			Heroes.AddRange(new[] {
				new Hero("Anti-Mage", HeroType.Agility, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Axe", HeroType.Strength, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Crystal_Maiden", HeroType.Intelligence, HeroDifficulty.One, HeroRole.Support, AttackType.Ranged),
				new Hero("Dazzle", HeroType.Intelligence, HeroDifficulty.One, HeroRole.Support, AttackType.Ranged),
				new Hero("Drow_Ranger", HeroType.Agility, HeroDifficulty.One, HeroRole.Core, AttackType.Ranged),
				new Hero("Earthshaker", HeroType.Strength, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Lich", HeroType.Intelligence, HeroDifficulty.One, HeroRole.Support, AttackType.Ranged),
				new Hero("Lina", HeroType.Intelligence, HeroDifficulty.One, HeroRole.Both, AttackType.Ranged),
				new Hero("Lion", HeroType.Intelligence, HeroDifficulty.One, HeroRole.Support, AttackType.Ranged),
				new Hero("Mirana", HeroType.Agility, HeroDifficulty.Two, HeroRole.Both, AttackType.Ranged),
				new Hero("Morphling", HeroType.Agility, HeroDifficulty.Three, HeroRole.Core, AttackType.Ranged),
				new Hero("Necrophos", HeroType.Intelligence, HeroDifficulty.One, HeroRole.Core, AttackType.Ranged),
				new Hero("Puck", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Core, AttackType.Ranged),
				new Hero("Pudge", HeroType.Strength, HeroDifficulty.Two, HeroRole.Both, AttackType.Melee),
				new Hero("Razor", HeroType.Agility, HeroDifficulty.One, HeroRole.Core, AttackType.Ranged),
				new Hero("Sand_King", HeroType.Strength, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Shadow_Shaman", HeroType.Intelligence, HeroDifficulty.One, HeroRole.Support, AttackType.Ranged),
				new Hero("Storm_Spirit", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Core, AttackType.Ranged),
				new Hero("Sven", HeroType.Strength, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Tidehunter", HeroType.Strength, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Vengeful_Spirit", HeroType.Agility, HeroDifficulty.One, HeroRole.Support, AttackType.Ranged),
				new Hero("Windranger", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Both, AttackType.Ranged),
				new Hero("Witch_Doctor", HeroType.Intelligence, HeroDifficulty.One, HeroRole.Support, AttackType.Ranged),
				new Hero("Zeus", HeroType.Intelligence, HeroDifficulty.One, HeroRole.Core, AttackType.Ranged),
				new Hero("Slardar", HeroType.Strength, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Enigma", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Support, AttackType.Ranged),
				new Hero("Faceless_Void", HeroType.Agility, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Tiny", HeroType.Strength, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Viper", HeroType.Agility, HeroDifficulty.One, HeroRole.Core, AttackType.Ranged),
				new Hero("Venomancer", HeroType.Agility, HeroDifficulty.One, HeroRole.Support, AttackType.Ranged),
				new Hero("Clockwerk", HeroType.Strength, HeroDifficulty.Two, HeroRole.Support, AttackType.Melee),
				new Hero("Natures_Prophet", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Core, AttackType.Ranged),
				new Hero("Dark_Seer", HeroType.Intelligence, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Sniper", HeroType.Agility, HeroDifficulty.One, HeroRole.Core, AttackType.Ranged),
				new Hero("Pugna", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Both, AttackType.Ranged),
				new Hero("Beastmaster", HeroType.Strength, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Enchantress", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Support, AttackType.Ranged),
				new Hero("Leshrac", HeroType.Intelligence, HeroDifficulty.One, HeroRole.Both, AttackType.Ranged),
				new Hero("Shadow_Fiend", HeroType.Agility, HeroDifficulty.Two, HeroRole.Core, AttackType.Ranged),
				new Hero("Tinker", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Core, AttackType.Ranged),
				new Hero("Weaver", HeroType.Agility, HeroDifficulty.Two, HeroRole.Both, AttackType.Ranged),
				new Hero("Night_Stalker", HeroType.Agility, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Ancient_Apparition", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Support, AttackType.Ranged),
				new Hero("Spectre", HeroType.Agility, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Doom", HeroType.Strength, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Chen", HeroType.Intelligence, HeroDifficulty.Three, HeroRole.Support, AttackType.Ranged),
				new Hero("Juggernaut", HeroType.Agility, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Bloodseeker", HeroType.Agility, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Kunkka", HeroType.Strength, HeroDifficulty.Two, HeroRole.Both, AttackType.Melee),
				new Hero("Riki", HeroType.Agility, HeroDifficulty.One, HeroRole.Both, AttackType.Melee),
				new Hero("Queen_of_Pain", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Both, AttackType.Ranged),
				new Hero("Wraith_King", HeroType.Strength, HeroDifficulty.One, HeroRole.Both, AttackType.Melee),
				new Hero("Broodmother", HeroType.Agility, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Huskar", HeroType.Strength, HeroDifficulty.One, HeroRole.Core, AttackType.Ranged),
				new Hero("Jakiro", HeroType.Intelligence, HeroDifficulty.One, HeroRole.Support, AttackType.Ranged),
				new Hero("Batrider", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Core, AttackType.Ranged),
				new Hero("Omniknight", HeroType.Strength, HeroDifficulty.One, HeroRole.Support, AttackType.Melee),
				new Hero("Dragon_Knight", HeroType.Strength, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Warlock", HeroType.Intelligence, HeroDifficulty.One, HeroRole.Support, AttackType.Ranged),
				new Hero("Alchemist", HeroType.Strength, HeroDifficulty.One, HeroRole.Both, AttackType.Melee),
				new Hero("Lifestealer", HeroType.Agility, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Death_Prophet", HeroType.Intelligence, HeroDifficulty.One, HeroRole.Core, AttackType.Ranged),
				new Hero("Ursa", HeroType.Agility, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Bounty_Hunter", HeroType.Agility, HeroDifficulty.One, HeroRole.Both, AttackType.Melee),
				new Hero("Silencer", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Both, AttackType.Ranged),
				new Hero("Spirit_Breaker", HeroType.Strength, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Invoker", HeroType.Intelligence, HeroDifficulty.Three, HeroRole.Core, AttackType.Ranged),
				new Hero("Clinkz", HeroType.Agility, HeroDifficulty.Two, HeroRole.Core, AttackType.Ranged),
				new Hero("Outworld_Destroyer", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Core, AttackType.Ranged),
				new Hero("Bane", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Support, AttackType.Ranged),
				new Hero("Shadow_Demon", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Support, AttackType.Ranged),
				new Hero("Lycan", HeroType.Strength, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Lone_Druid", HeroType.Agility, HeroDifficulty.Three, HeroRole.Core, AttackType.Ranged),
				new Hero("Brewmaster", HeroType.Strength, HeroDifficulty.Three, HeroRole.Core, AttackType.Melee),
				new Hero("Phantom_Lancer", HeroType.Agility, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Treant_Protector", HeroType.Strength, HeroDifficulty.Two, HeroRole.Support, AttackType.Melee),
				new Hero("Ogre_Magi", HeroType.Intelligence, HeroDifficulty.One, HeroRole.Support, AttackType.Melee),
				new Hero("Gyrocopter", HeroType.Agility, HeroDifficulty.One, HeroRole.Core, AttackType.Ranged),
				new Hero("Chaos_Knight", HeroType.Strength, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Phantom_Assassin", HeroType.Agility, HeroDifficulty.One, HeroRole.Core, AttackType.Ranged),
				new Hero("Rubick", HeroType.Intelligence, HeroDifficulty.Three, HeroRole.Support, AttackType.Ranged),
				new Hero("Luna", HeroType.Agility, HeroDifficulty.One, HeroRole.Core, AttackType.Ranged),
				new Hero("Io", HeroType.Strength, HeroDifficulty.Three, HeroRole.Support, AttackType.Ranged),
				new Hero("Undying", HeroType.Strength, HeroDifficulty.One, HeroRole.Both, AttackType.Melee),
				new Hero("Disruptor", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Support, AttackType.Ranged),
				new Hero("Templar_Assassin", HeroType.Agility, HeroDifficulty.Two, HeroRole.Core, AttackType.Ranged),
				new Hero("Naga_Siren", HeroType.Agility, HeroDifficulty.Two, HeroRole.Both, AttackType.Melee),
				new Hero("Nyx_Assassin", HeroType.Agility, HeroDifficulty.Two, HeroRole.Support, AttackType.Melee),
				new Hero("Keeper_of_the_Light", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Support, AttackType.Ranged),
				new Hero("Visage", HeroType.Intelligence, HeroDifficulty.One, HeroRole.Support, AttackType.Ranged),
				new Hero("Meepo", HeroType.Agility, HeroDifficulty.Three, HeroRole.Core, AttackType.Melee),
				new Hero("Magnus", HeroType.Strength, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Centaur_Warrunner", HeroType.Strength, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Slark", HeroType.Agility, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Timbersaw", HeroType.Strength, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Medusa", HeroType.Agility, HeroDifficulty.One, HeroRole.Core, AttackType.Ranged),
				new Hero("Troll_Warlord", HeroType.Agility, HeroDifficulty.Two, HeroRole.Core, AttackType.Ranged),
				new Hero("Tusk", HeroType.Strength, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Bristleback", HeroType.Strength, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Skywrath_Mage", HeroType.Intelligence, HeroDifficulty.One, HeroRole.Support, AttackType.Ranged),
				new Hero("Elder_Titan", HeroType.Strength, HeroDifficulty.Two, HeroRole.Both, AttackType.Melee),
				new Hero("Abaddon", HeroType.Strength, HeroDifficulty.One, HeroRole.Both, AttackType.Melee),
				new Hero("Ember_Spirit", HeroType.Agility, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Earth_Spirit", HeroType.Strength, HeroDifficulty.Three, HeroRole.Support, AttackType.Melee),
				new Hero("Legion_Commander", HeroType.Strength, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Phoenix", HeroType.Strength, HeroDifficulty.Two, HeroRole.Support, AttackType.Ranged),
				new Hero("Terrorblade", HeroType.Agility, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Techies", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Support, AttackType.Ranged),
				new Hero("Oracle", HeroType.Intelligence, HeroDifficulty.Three, HeroRole.Support, AttackType.Ranged),
				new Hero("Winter_Wyvern", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Support, AttackType.Ranged),
				new Hero("Arc_Warden", HeroType.Agility, HeroDifficulty.Three, HeroRole.Core, AttackType.Ranged),
				new Hero("Underlord", HeroType.Strength, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Monkey_King", HeroType.Agility, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Dark_Willow", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Support, AttackType.Ranged),
				new Hero("Pangolier", HeroType.Agility, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Grimstroke", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Support, AttackType.Ranged),
				new Hero("Mars", HeroType.Strength, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Snapfire", HeroType.Intelligence, HeroDifficulty.One, HeroRole.Support, AttackType.Ranged),
				new Hero("Void_Spirit", HeroType.Intelligence, HeroDifficulty.Two, HeroRole.Core, AttackType.Melee),
				new Hero("Hoodwink", HeroType.Agility, HeroDifficulty.Two, HeroRole.Both, AttackType.Ranged),
				new Hero("Dawnbreaker", HeroType.Strength, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
				new Hero("Marci", HeroType.Strength, HeroDifficulty.Two, HeroRole.Both, AttackType.Melee),
				new Hero("Primal_Beast", HeroType.Strength, HeroDifficulty.One, HeroRole.Core, AttackType.Melee),
			});
		}
	}

	public class FilterData {
		public int StrengthWeight { get; set; }
		public bool StrengthEnabled { get; set; }
		public bool IsStrengthValid() => StrengthEnabled && StrengthWeight > 0;
		public int GetStrengthWeight() => StrengthEnabled ? StrengthWeight : 0;
		public int AgilityWeight { get; set; }
		public bool AgilityEnabled { get; set; }
		public bool IsAgilityValid() => AgilityEnabled && AgilityWeight > 0;
		public int GetAgilityWeight() => AgilityEnabled ? AgilityWeight : 0;
		public int IntelligenceWeight { get; set; }
		public bool IntelligenceEnabled { get; set; }
		public bool IsIntelligenceValid() => IntelligenceEnabled && IntelligenceWeight > 0;
		public int GetIntelligenceWeight() => IntelligenceEnabled ? IntelligenceWeight : 0;
		public int DifficultyOneWeight { get; set; }
		public bool DifficultyOneEnabled { get; set; }
		public bool IsDifficultyOneValid() => DifficultyOneEnabled && DifficultyOneWeight > 0;
		public int GetDifficultyOneWeight() => DifficultyOneEnabled ? DifficultyOneWeight : 0;
		public int DifficultyTwoWeight { get; set; }
		public bool DifficultyTwoEnabled { get; set; }
		public bool IsDifficultyTwoValid() => DifficultyTwoEnabled && DifficultyTwoWeight > 0;
		public int GetDifficultyTwoWeight() => DifficultyTwoEnabled ? DifficultyTwoWeight : 0;
		public int DifficultyThreeWeight { get; set; }
		public bool DifficultyThreeEnabled { get; set; }
		public bool IsDifficultyThreeValid() => DifficultyThreeEnabled && DifficultyThreeWeight > 0;
		public int GetDifficultyThreeWeight() => DifficultyThreeEnabled ? DifficultyThreeWeight : 0;
		public int CoreWeight { get; set; }
		public bool CoreEnabled { get; set; }
		public bool IsCoreValid() => CoreEnabled && CoreWeight > 0;
		public int GetCoreWeight() => CoreEnabled ? CoreWeight : 0;
		public int SupportWeight { get; set; }
		public bool SupportEnabled { get; set; }
		public bool IsSupportValid() => SupportEnabled && SupportWeight > 0;
		public int GetSupportWeight() => SupportEnabled ? SupportWeight : 0;
		public int RangedWeight { get; set; }
		public bool RangedEnabled { get; set; }
		public bool IsRangedValid() => RangedEnabled && RangedWeight > 0;
		public int GetRangedWeight() => RangedEnabled ? RangedWeight : 0;
		public int MeleeWeight { get; set; }
		public bool MeleeEnabled { get; set; }
		public bool IsMeleeValid() => MeleeEnabled && MeleeWeight > 0;
		public int GetMeleeWeight() => MeleeEnabled ? MeleeWeight : 0;
		public int Amount { get; set; }

		public FilterData() { }
	}

	public class Hero {
		public string Name { get; set; }
		public HeroType HeroType { get; set; }
		public HeroDifficulty HeroDifficulty { get; set; }
		public HeroRole HeroRole { get; set; }
		public AttackType AttackType { get; set; }

		public bool IsEnabled { get; set; } = true;
		public int Weight { get; set; } = 100;

		public bool IsValid() => IsEnabled && Weight > 0;
		public int GetWeight() => IsEnabled ? Weight : 0;
		public string GetFixedHeroName() => Name.Replace("-", " ").Replace("_", " ");

		public Hero() { }
		public Hero(string name, HeroType heroType, HeroDifficulty heroDifficulty, HeroRole heroRole, AttackType attackType) {
			Name = name;
			HeroType = heroType;
			HeroDifficulty = heroDifficulty;
			HeroRole = heroRole;
			AttackType = attackType;
		}

		public override string ToString() {
			return $"{Name}";
		}
	}

	public enum HeroType {
		Strength, Agility, Intelligence
	}

	public enum HeroDifficulty {
		One, Two, Three
	}

	public enum HeroRole {
		Core, Support, Both
	}

	public enum AttackType {
		Melee, Ranged
	}
}

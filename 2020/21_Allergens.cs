using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code._2020
{
    class _21_Allergens : AoCDay
    {
        protected override void Run()
        {
            (string[] ingredients, string[] allergens)[] foods =
                new (string[], string[])[inputLines.Length];
            List<string> allIngredients = new(), allAllergens = new();
            for (int i = 0; i < inputLines.Length; i++)
            {
                string[] split = inputLines[i][..^1].Split(" (contains ");
                foods[i] = (split[0].Split(' '), split[1].Split(", "));
                allIngredients.AddRange(foods[i].ingredients);
                allAllergens.AddRange(foods[i].allergens);
            }
            allIngredients = allIngredients.Distinct().ToList();
            allAllergens = allAllergens.Distinct().ToList();

            Array.Sort(foods, (f1, f2) => f1.allergens.Length - f2.allergens.Length);
            Dictionary<string, List<string>> match = new();
            foreach ((string[] ingredients, string[] allergens) in foods)
                foreach (string allergen in allergens)
                    if (!match.ContainsKey(allergen))
                        match[allergen] = ingredients.ToList();
                    else match[allergen] = match[allergen].Intersect(ingredients).ToList();

            do
                foreach (string allergen in allAllergens)
                    if (match[allergen].Count == 1)
                        foreach ((string a, List<string> i) in match)
                            if (a != allergen)
                                i.Remove(match[allergen][0]);
            while (match.Values.Any(l => l.Count != 1));

            (string allergen, string ingredient)[] dangerousFoods =
                Array.ConvertAll(match.ToArray(), m => (m.Key, m.Value[0]));
            string[] dangerousIngredients = Array.ConvertAll
                (dangerousFoods, f => f.ingredient),
                inertIngredients = allIngredients.Except(dangerousIngredients).ToArray();
            foreach ((string[] ingredients, _) in foods)
                part1 += Array.FindAll(ingredients, i => inertIngredients.Contains(i)).LongLength;
            Array.Sort(dangerousFoods, (f1, f2) => string.Compare(f1.allergen, f2.allergen));
            part2_str = string.Join(",", Array.ConvertAll(dangerousFoods, f => f.ingredient));
        }
    }
}

using System.Text.RegularExpressions;

var input = File.ReadLines("./input");

// var input = new string [] {
//     "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
//     "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",
//     "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",
//     "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
//     "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green",
//     "Game 6: 6 red, 3 green; 1 red, 2 green"
// };

Part1(input);
Part2(input);

static void Part1(IEnumerable<string> input) {
    var limits = new Dictionary<string, int>() {
        {"red", 12},
        {"green", 13},
        {"blue", 14}
    };

    var gameId = 1;
    var possibleGameIdSum = 0;

    foreach (var game in input) {
        if (isGamePossible(game, limits)) possibleGameIdSum += gameId;
        
        gameId++;
    }

    Console.WriteLine(possibleGameIdSum);
}

static void Part2(IEnumerable<string> input) {
    var gamePowerSum = 0;
    foreach (var game in input) {
        var highestPulls = new Dictionary<string, int>();
        
        foreach (var color in new string[] {"red", "green", "blue"}) {
            highestPulls.Add(color, highestColorPull(game, color));
        }

        gamePowerSum += gamePower(highestPulls);
    }

    Console.WriteLine(gamePowerSum);
}

static string[] getPulls(string game) {
    return game.Substring(
            game.IndexOf(':') + 1
        )
        .Split(';')
        .Select(pull => pull.Trim())
        .ToArray();
}

static int cubeCount(string pull, string color) {
    var regex = new Regex($"(?'count'[0-9]*) {color}");
    var countGroupValue = regex.Match(pull).Groups["count"].Value.Trim();
    
    var count = 0;
    int.TryParse(countGroupValue, out count);
    
    return count;
}

static bool isGamePossible(string game, Dictionary<string, int> cubeLimits) {
    var pulls = getPulls(game);
    
    foreach (var pull in pulls) {
        foreach (var color in new string[] {"red", "green", "blue"}) {
            if (cubeCount(pull, color) > cubeLimits[color]) return false;
        }
    }

    return true;
}

static int highestColorPull(string game, string color) {
    var regex = new Regex($"(?'count'[0-9]*) {color}");
    
    if (!regex.Matches(game).Any()) return 0;

    var highestColorPull = regex.Matches(game).Select(match => {
        var count = 0;
        int.TryParse(match.Groups["count"].Value, out count);
    
        return count;
    }).Max();
    
    
    return highestColorPull;
}

static int gamePower(Dictionary<string, int> highestColorPulls) {
    return highestColorPulls.Values.Aggregate((value, next) => value*next);
}
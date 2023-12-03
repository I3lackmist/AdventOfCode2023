using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

static void Part1(IEnumerable<string> input) {
    var nonDigitRegex = new Regex("[^0-9]");
    var calibrationValueSum = 0;

    foreach (var line in input) {
        var digitsInCurrentLine = nonDigitRegex.Replace(line, "");
        var calibrationValue = string.Concat(digitsInCurrentLine.First(), digitsInCurrentLine.Last());

        calibrationValueSum += int.Parse(calibrationValue);
    }

    Console.WriteLine(calibrationValueSum);
}

static void Part2(IEnumerable<string> input) {
    Dictionary<string, string> digitsSpelled = new Dictionary<string, string> {
        {"one", "1"},
        {"two", "2"},
        {"three", "3"},
        {"four", "4"},
        {"five", "5"},
        {"six", "6"},
        {"seven", "7"},
        {"eight", "8"},
        {"nine", "9"}
    };

    var digitRegex = new Regex(
        "(one)|(two)|(three)|(four)|(five)|(six)|(seven)|(eight)|(nine)|[1-9]"
    );

    var digitRegexRightToLeft = new Regex(
        digitRegex.ToString(), RegexOptions.RightToLeft
    );

    var calibrationValueSum = 0;
    var index = 1;

    foreach (var line in input) {
        var firstMatch = digitRegex.Matches(line).First().Value;
        var lastMatch = digitRegexRightToLeft.Matches(line).First().Value;

        var tens = int.Parse(digitsSpelled.Keys.Contains(firstMatch) ? digitsSpelled[firstMatch] : firstMatch) * 10;
        var singles = int.Parse(digitsSpelled.Keys.Contains(lastMatch) ? digitsSpelled[lastMatch] : lastMatch);
        
        var calibrationValue = tens + singles;

        if (calibrationValue <= 0) 
            throw new ArgumentException($"Calibration value is less than or equal 0.\nLine: {line}\nValue: {calibrationValue}");
        
        if (calibrationValue < 11) 
            throw new ArgumentException($"Calibration value is less than 11.\nLine: {line}\nValue: {calibrationValue}");

        Console.WriteLine(
            $"{index++}: {line} => {calibrationValue}\n"+
            $"{calibrationValueSum} + {calibrationValue} = {calibrationValue+calibrationValueSum}\n"
        );

        calibrationValueSum += calibrationValue;
    }

    Console.WriteLine(calibrationValueSum);
}

var exampleInput = new string[] {
    "two1nine",
    "eightwothree",
    "abcone2threexyz",
    "xtwone3four",
    "4nineeightseven2",
    "zoneight234",
    "7pqrstsixteen"
};

var input = File.ReadLines("./input");

Part2(input);
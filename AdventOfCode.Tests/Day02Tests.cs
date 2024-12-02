namespace AdventOfCode.Tests
{
    public class Day02Tests
    {
        [Fact]
        public async Task Part2Test_01()
        {
            // Arrange
            Day02Mock day02 = new Day02Mock(
                [
                    "19 21 24 27 24", //1
                    "85 87 89 92 93 96 98 98", //1
                    "2 5 6 7 8 12", //1
                    "63 66 69 72 75 82", //1
                    "18 21 23 26 28 26 27 28", //0
                    "16 19 21 19 20 22 23 22", //0
                    "37 39 37 38 41 42 44 44", //0
                    "18 20 17 20 24", //0
                    "50 51 54 53 58" //0
                ]
            );

            // Act
            var result = await day02.Solve_2();

            // Assert
            Assert.Equal("4", result);
        }

        [Fact]
        public async Task Part2Test_02()
        {
            // Arrange
            Day02Mock day02 = new Day02Mock(
                [
                    "73 76 77 80 83 83 84 85", //1
                    "30 31 33 33 36 39 40 37", //0
                    "50 52 52 53 54 55 55", //0
                    "49 52 54 56 59 59 63", //0
                    "42 44 46 46 49 52 53 59", //0
                    "40 43 45 49 52 54", //0
                    "87 89 93 96 95", //0
                    "66 69 70 71 73 77 78 78", //0
                    "30 32 33 36 39 43 47", //0
                    "8 9 12 16 18 21 22 28", //0
                ]
            );

            // Act
            var result = await day02.Solve_2();

            // Assert
            Assert.Equal("1", result);
        }
    }

    public class Day02Mock : Day02
    {
        private string[] _lines;

        public Day02Mock(string[] lines)
        {
            _lines = lines;
        }

        public override string[] GetLines()
        {
            return _lines;
        }
    }
}

using aocTools;

namespace aoc22.day4;

public class Day4 : AAocDay {
    public Day4() {
        InputTokens = InputTokens.RemoveEmptyTokens().ToTokenList();
    }

    private List<ElfPair> _pairs = new List<ElfPair>();
    public override void PuzzleOne() {
        while (InputTokens.HasMoreTokens()) {
            var split = InputTokens.Read().Split(new[] {',', '-'});
            _pairs.Add(new ElfPair(split[0], split[1], split[2], split[3]));
        }

        Console.WriteLine(_pairs.Count(elfPair => elfPair.SectionsFullyOverlap()));
    }

    public override void PuzzleTwo() {
        Console.WriteLine(_pairs.Count(elfPair => elfPair.SectionsPartiallyOverlap()));
    }

    public class ElfPair {
        public int Elf1SectionBegin { get; set; }
        public int Elf1SectionEnd { get; set; }
        public int Elf2SectionBegin { get; set; }
        public int Elf2SectionEnd { get; set; }

        public ElfPair(int elf1SectionBegin, int elf1SectionEnd, int elf2SectionBegin, int elf2SectionEnd) {
            Elf1SectionBegin = elf1SectionBegin;
            Elf1SectionEnd = elf1SectionEnd;
            Elf2SectionBegin = elf2SectionBegin;
            Elf2SectionEnd = elf2SectionEnd;
        }

        public ElfPair(string elf1SectionBegin, string elf1SectionEnd, string elf2SectionBegin, string elf2SectionEnd) {
            Elf1SectionBegin = int.Parse(elf1SectionBegin);
            Elf1SectionEnd = int.Parse(elf1SectionEnd);
            Elf2SectionBegin = int.Parse(elf2SectionBegin);
            Elf2SectionEnd = int.Parse(elf2SectionEnd);
        }

        public bool SectionsFullyOverlap() {
            // if one section fully contains the other, they overlap
            if (Elf1SectionBegin <= Elf2SectionBegin && Elf1SectionEnd >= Elf2SectionEnd) 
                return true;
            if (Elf2SectionBegin <= Elf1SectionBegin && Elf2SectionEnd >= Elf1SectionEnd) 
                return true;
            
            return false;
        }
        public bool SectionsPartiallyOverlap() {
            // if one section is completely to the left of the other, they don't overlap
            if (Elf1SectionEnd < Elf2SectionBegin || Elf2SectionEnd < Elf1SectionBegin) 
                return false;
            
            return true;
        }
    }
}
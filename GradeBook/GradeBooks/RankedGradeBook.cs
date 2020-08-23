using GradeBook.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GradeBook.GradeBooks
{
    public class RankedGradeBook : BaseGradeBook
    {
        public RankedGradeBook(string name) : base(name)
        {
            Type = GradeBookType.Ranked;
        }

        public override char GetLetterGrade(double averageGrade)
        {
            if (Students.Count < 5)
            {
                throw new InvalidOperationException("Ranked-grading requires a minimum of 5 students to work.");
            }

            var twentyPercentOfStudents = Students.Count * 20 / 100;
            var rankedStudents = new Dictionary<char, List<Student>> 
            {
                ['A'] = new List<Student>(),
                ['B'] = new List<Student>(),
                ['C'] = new List<Student>(),
                ['D'] = new List<Student>(),
                ['E'] = new List<Student>(),
            };

            var rankToStudentsMapEnumerator = rankedStudents.GetEnumerator();

            foreach(var student in Students.OrderByDescending(x => x.AverageGrade))
            {
                var currentRank = rankToStudentsMapEnumerator.Current;
                if (currentRank.Value.Count >= twentyPercentOfStudents)
                {
                    rankToStudentsMapEnumerator.MoveNext();
                }

                currentRank.Value.Add(student);
            }

            foreach(var rank in rankedStudents)
            {
                if(averageGrade >= rank.Value.Min(x => x.AverageGrade))
                {
                    return rank.Key;
                }
            }

            return 'F';
        }
    }
}

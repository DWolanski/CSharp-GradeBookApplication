using GradeBook.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GradeBook.GradeBooks
{
    public class RankedGradeBook : BaseGradeBook
    {
        public RankedGradeBook(string name, bool isWeighted) : base(name, isWeighted)
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
            var rankedStudents = new SortedDictionary<char, List<Student>>
            {
                ['A'] = new List<Student>(),
                ['B'] = new List<Student>(),
                ['C'] = new List<Student>(),
                ['D'] = new List<Student>(),
                ['F'] = new List<Student>(),
            };

            var rankToStudentsMapEnumerator = rankedStudents.GetEnumerator();
            rankToStudentsMapEnumerator.MoveNext();

            foreach(var student in Students.OrderByDescending(x => x.AverageGrade))
            {
                if (rankToStudentsMapEnumerator.Current.Value.Count.Equals(twentyPercentOfStudents))
                {
                    rankToStudentsMapEnumerator.MoveNext();
                }

                rankToStudentsMapEnumerator.Current.Value.Add(student);
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

        public override void CalculateStatistics()
        {
            if(Students.Count < 5)
            {
                Console.WriteLine("Ranked grading requires at least 5 students with grades in order to properly calculate a student's overall grade.");
                return;
            }
                
            base.CalculateStatistics();
        }

        public override void CalculateStudentStatistics(string name)
        {
            if(Students.Count < 5)
            {
                Console.WriteLine("Ranked grading requires at least 5 students with grades in order to properly calculate a student's overall grade.");
                return;
            }

            base.CalculateStudentStatistics(name);
        }
    }
}

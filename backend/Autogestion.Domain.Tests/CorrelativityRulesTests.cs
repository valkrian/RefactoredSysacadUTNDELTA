using Autogestion.Domain.Entities;
using Autogestion.Domain.Enums;
using Autogestion.Domain.Services;
using Xunit;

namespace Autogestion.Domain.Tests;

/// <summary>
/// Unit tests for correlativity rules in the domain layer.
/// </summary>
public class CorrelativityRulesTests
{
    /// <summary>
    /// Course enrollment is allowed when prerequisite is regular.
    /// </summary>
    [Fact]
    public void CanEnrollCourse_Allows_WhenPrerequisiteIsRegular()
    {
        var prerequisites = new[]
        {
            new Prerequisite
            {
                SubjectId = 10,
                RequiresSubjectId = 1,
                Type = PrerequisiteType.ForCourse,
                MinimumStatus = MinimumSubjectStatus.Regular
            }
        };

        var courseEnrollments = new[]
        {
            new CourseEnrollment
            {
                SubjectId = 1,
                Status = CourseEnrollmentStatus.Regular
            }
        };

        var canEnroll = CorrelativityRules.CanEnrollCourse(
            10,
            prerequisites,
            courseEnrollments,
            Array.Empty<ExamResult>());

        Assert.True(canEnroll);
    }

    /// <summary>
    /// Course enrollment is denied when only enrolled and not regular.
    /// </summary>
    [Fact]
    public void CanEnrollCourse_Denies_WhenOnlyEnrolled()
    {
        var prerequisites = new[]
        {
            new Prerequisite
            {
                SubjectId = 10,
                RequiresSubjectId = 1,
                Type = PrerequisiteType.ForCourse,
                MinimumStatus = MinimumSubjectStatus.Regular
            }
        };

        var courseEnrollments = new[]
        {
            new CourseEnrollment
            {
                SubjectId = 1,
                Status = CourseEnrollmentStatus.Enrolled
            }
        };

        var canEnroll = CorrelativityRules.CanEnrollCourse(
            10,
            prerequisites,
            courseEnrollments,
            Array.Empty<ExamResult>());

        Assert.False(canEnroll);
    }

    /// <summary>
    /// Exam enrollment is allowed when prerequisite is approved.
    /// </summary>
    [Fact]
    public void CanEnrollExam_Allows_WhenPrerequisiteIsApproved()
    {
        var prerequisites = new[]
        {
            new Prerequisite
            {
                SubjectId = 10,
                RequiresSubjectId = 1,
                Type = PrerequisiteType.ForExam,
                MinimumStatus = MinimumSubjectStatus.Approved
            }
        };

        var examResults = new[]
        {
            new ExamResult
            {
                SubjectId = 1,
                Status = ExamResultStatus.Approved
            }
        };

        var canEnroll = CorrelativityRules.CanEnrollExam(
            10,
            prerequisites,
            Array.Empty<CourseEnrollment>(),
            examResults);

        Assert.True(canEnroll);
    }

    /// <summary>
    /// Exam enrollment is denied when only regular and not approved.
    /// </summary>
    [Fact]
    public void CanEnrollExam_Denies_WhenOnlyRegular()
    {
        var prerequisites = new[]
        {
            new Prerequisite
            {
                SubjectId = 10,
                RequiresSubjectId = 1,
                Type = PrerequisiteType.ForExam,
                MinimumStatus = MinimumSubjectStatus.Approved
            }
        };

        var courseEnrollments = new[]
        {
            new CourseEnrollment
            {
                SubjectId = 1,
                Status = CourseEnrollmentStatus.Regular
            }
        };

        var canEnroll = CorrelativityRules.CanEnrollExam(
            10,
            prerequisites,
            courseEnrollments,
            Array.Empty<ExamResult>());

        Assert.False(canEnroll);
    }
}

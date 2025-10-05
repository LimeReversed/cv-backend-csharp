using BackendCSharp.Models;
using Microsoft.Data.Sqlite;
using System.Diagnostics;

namespace BackendCSharp.Repositories;

using BackendCSharp.Database;

public static class RepositoryHelpers
{
    public static List<Experience> FillExperience(List<Experience> experience)
    {
        using var connection = new SqliteConnection(DatabaseServiceGeneric.connectionString);
        
        try
        {
            var experienceIds = experience.Select(x => x.Id).ToList();

            connection.Open();
            using var transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable);
            var projectsByExperienceIds = ProjectRepository.GetByExperienceIds(experienceIds);
            var tagsByExperienceIds = TagRepository.GetByExperienceIds(experienceIds);

            var projectIds = projectsByExperienceIds.SelectMany(keyValuePair => keyValuePair.Value).Select(project => project.Id).ToList();
            var imagePathsByProjectIds = ImagePathRepository.GetByProjectIds(projectIds);

            foreach (Experience ex in experience)
            {
                // Some experiences don't have projects, so some experienceIDs will not be in represented in projectsByExperienceIds.
                // So we first need to check if the projectsByExperienceIds dictionary contains that experienceId. 
                if (projectsByExperienceIds.ContainsKey(ex.Id)) { ex.Projects = projectsByExperienceIds[ex.Id]; }
                if (tagsByExperienceIds.ContainsKey(ex.Id)) { ex.Tags = tagsByExperienceIds[ex.Id]; }

                foreach (Project project in ex.Projects)
                {
                    if (imagePathsByProjectIds.ContainsKey(project.Id)) { project.Imagepaths = imagePathsByProjectIds[project.Id]; }
                }
            }
        }
        catch (SqliteException e)
        {
            Debug.WriteLine(e.Message);
        }

        return experience;
    }
}
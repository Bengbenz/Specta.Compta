using System;

using Beng.Specta.Compta.Core.Entities;

namespace Beng.Specta.Compta.UnitTests.Helpers
{
    public class ProjectBuilder
    {
        private Project _project;

        public ProjectBuilder()
        {
            _project = new Project();
        }

        public ProjectBuilder SetId(long id)
        {
            _project.Id = id;
            return this;
        }

        public ProjectBuilder SetCode(string code)
        {
            _project.Code = code;
            return this;
        }

        public ProjectBuilder SetName(string name)
        {
            _project.Name = name;
            return this;
        }

        public ProjectBuilder SetDescription(string description)
        {
            _project.Description = description;
            return this;
        }

        public ProjectBuilder SetDuration(int duration)
        {
            _project.Duration = duration;
            return this;
        }

        public ProjectBuilder SetStartDate(DateTime time)
        {
            _project.StartDate = time;
            return this;
        }

        public ProjectBuilder WithDefaultValues()
        {
            _project = new Project
            {
                Code="Test",
                Name = "Test Item",
                Description = "Test Description",
                StartDate = DateTime.UtcNow
            };

            return this;
        }

        public Project Build()
        {
            return _project;
        }
    }
}

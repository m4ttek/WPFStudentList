using StudentList.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsList.model
{
    public class StorageContext : DbContext
    {
        public StorageContext() : base("StudentsDatabase") {}
        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
    }

    public class Storage
    {
        public Storage()
        {
            MigrateDatabaseToLatestVersion<StorageContext, Configuration> di = new MigrateDatabaseToLatestVersion<StorageContext, Configuration>();
            Database.SetInitializer(di);
        }
     
        public List<Student> getStudents(int groupId)
        {
            using (var db = new StorageContext())
                return db.Students.Where(
                    student => student.Group.GroupId == groupId)
                                  .ToList();
        }

        public List<Group> getGroups()
        {
            using (var db = new StorageContext())
                return db.Groups.ToList();
        }

        public void createStudent(string firstName, string lastName, string indexNo, int groupId)
        {
            using (var db = new StorageContext())
            {
                var group = db.Groups.Find(groupId);
                var student = new Student { FirstName = firstName, LastName = lastName, IndexNo = indexNo, Group = group };
                db.Students.Add(student);
                db.SaveChanges();
            }
        }

        public void updateStudent(Student st)
        {
            using (var db = new StorageContext())
            {
                var original = db.Students.Find(st.StudentId);
                if (original != null)
                {
                    original.FirstName = st.FirstName;
                    original.LastName = st.LastName;
                    int groupId = original.Group.GroupId;
                    original.IndexNo = st.IndexNo;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        Console.WriteLine(ex.EntityValidationErrors.ElementAt(0).ValidationErrors.ElementAt(0).ErrorMessage);
                    }
                }
            }
        }

        public void deleteStudent(Student st)
        {
            using (var db = new StorageContext())
            {
                var original = db.Students.Find(st.StudentId);
                if (original != null)
                {
                    db.Students.Remove(original);
                    db.SaveChanges();
                }
            }
        }
    }
}

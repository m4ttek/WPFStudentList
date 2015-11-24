using StudentList;
using StudentsList.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentsList.viewmodel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand ButtonCommand { get; set; }

        public List<Group> GroupList { get; set; }

        private List<Student> studentsList;
        public List<Student> StudentsList
        {
            get { return studentsList; }
            set
            {
                if (value == studentsList)
                {
                    return;
                }
                studentsList = value;
                OnPropertyChanged("StudentsList");
            }
        }

        private Student chosenStudent;
        public Student ChosenStudent
        {
            get { return chosenStudent; }
            set
            {
                if (value == chosenStudent)
                {
                    return;
                }
                chosenStudent = value;
                applyChosenStudent();
            }
        }

        private String studentName;
        public String StudentName
        {
            get { return studentName; }
            set
            {
                if (value == studentName)
                {
                    return;
                }
                studentName = value;
                inputFieldsUpdate();
                OnPropertyChanged("StudentName");
            }
        }

        private String studentSurname;
        public String StudentSurname
        {
            get { return studentSurname; }
            set
            {
                if (value == studentSurname)
                {
                    return;
                }
                studentSurname = value;
                inputFieldsUpdate();
                OnPropertyChanged("StudentSurname");
            }
        }

        private String studentIndex;
        public String StudentIndex
        {
            get { return studentIndex; }
            set
            {
                if (value == studentIndex)
                {
                    return;
                }
                studentIndex = value;
                inputFieldsUpdate();
                OnPropertyChanged("StudentIndex");
            }
        }

        private Group selectedGroup;
        public Group SelectedGroup
        {
            get { return selectedGroup; }
            set {
                if (value == selectedGroup)
                {
                    return;
                }
                selectedGroup = value;
                updateCurrentGroupView();
                OnPropertyChanged("SelectedGroup");
            }
        }

        private Boolean isNewButtonEnabled;
        public Boolean IsNewButtonEnabled
        {
            get { return isNewButtonEnabled; }
            set
            {
                if (value == isNewButtonEnabled)
                {
                    return;
                }
                isNewButtonEnabled = value;
                OnPropertyChanged("IsNewButtonEnabled");
            }
        }

        private Boolean showNewButtonTooltip;
        public Boolean ShowNewButtonTooltip
        {
            get { return showNewButtonTooltip; }
            set
            {
                if (value == showNewButtonTooltip)
                {
                    return;
                }
                showNewButtonTooltip = value;
                OnPropertyChanged("ShowNewButtonTooltip");
            }
        }

        private Boolean isSaveButtonEnabled;
        public Boolean IsSaveButtonEnabled
        {
            get { return isSaveButtonEnabled; }
            set
            {
                if (value == isSaveButtonEnabled)
                {
                    return;
                }
                isSaveButtonEnabled = value;
                OnPropertyChanged("IsSaveButtonEnabled");
            }
        }

        private Boolean showSaveButtonTooltip;
        public Boolean ShowSaveButtonTooltip
        {
            get { return showSaveButtonTooltip; }
            set
            {
                if (value == showSaveButtonTooltip)
                {
                    return;
                }
                showSaveButtonTooltip = value;
                OnPropertyChanged("ShowSaveButtonTooltip");
            }
        }

        private Boolean isDeleteButtonEnabled;
        public Boolean IsDeleteButtonEnabled
        {
            get { return isDeleteButtonEnabled; }
            set
            {
                if (value == isDeleteButtonEnabled)
                {
                    return;
                }
                isDeleteButtonEnabled = value;
                OnPropertyChanged("IsDeleteButtonEnabled");
            }
        }

        private Boolean showDeleteButtonTooltip;
        public Boolean ShowDeleteButtonTooltip
        {
            get { return showDeleteButtonTooltip; }
            set
            {
                if (value == showDeleteButtonTooltip)
                {
                    return;
                }
                showDeleteButtonTooltip = value;
                OnPropertyChanged("ShowDeleteButtonTooltip");
            }
        }

        /**
         * Miejsce, gdzie trzymane są wszystkie dane.
         */
        private Storage storage;

        public MainWindowViewModel()
        {
            ButtonCommand = new RelayCommand(pars => buttonClicked(pars));
            
            storage = new Storage();
            GroupList = storage.getGroups();
            if (GroupList.Count != 0)
            {
                SelectedGroup = GroupList[0];
                StudentsList = storage.getStudents(SelectedGroup.GroupId);
            }
            clearInputFields();
        }

        public void buttonClicked(object par)
        {
            switch ((String) par) {
                case "delete":
                    storage.deleteStudent(chosenStudent);
                    break;
                case "save":
                    chosenStudent.IndexNo = StudentIndex;
                    chosenStudent.FirstName = StudentName;
                    chosenStudent.LastName = studentSurname;
                    storage.updateStudent(chosenStudent);
                    break;
                case "new":
                    storage.createStudent(studentName, studentSurname, StudentIndex, SelectedGroup.GroupId);
                    break;
                default:
                    throw new Exception("Unknown button command parameter!");
            }
            updateCurrentGroupView();
        }

        /**
         * Wywoływana, jeżeli zajdzie zmiana w polach wejściowych.
         */
        public void inputFieldsUpdate()
        {
            ShowNewButtonTooltip = !(IsNewButtonEnabled 
                = studentName != null && studentName.Length != 0 &&
                    studentSurname != null && studentSurname.Length != 0 &&
                    studentIndex != null && studentIndex.Length != 0);
            if (chosenStudent != null)
            {
                IsSaveButtonEnabled = IsNewButtonEnabled;
                ShowSaveButtonTooltip = ShowNewButtonTooltip;
            }
        }

        /**
         * Wywoływana, jeśli zostanie wybrany student z listy.
         */
        public void applyChosenStudent()
        {
            if (chosenStudent != null)
            {
                StudentName = chosenStudent.FirstName;
                StudentSurname = chosenStudent.LastName;
                StudentIndex = chosenStudent.IndexNo;
                IsDeleteButtonEnabled = true;
                ShowDeleteButtonTooltip = false;
            }
            else
            {
                IsSaveButtonEnabled = false;
                IsDeleteButtonEnabled = false;
                ShowDeleteButtonTooltip = true;
            }
        }

        /**
         * Wywoływana w przypadku aktualizacji stanu grupy.
         */
        public void updateCurrentGroupView()
        {
            if (selectedGroup != null)
            {
                StudentsList = storage.getStudents(selectedGroup.GroupId);
                ChosenStudent = null;
                clearInputFields();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = null;
        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private void clearInputFields()
        {
            StudentName = null;
            StudentSurname = null;
            StudentIndex = null;
            ShowNewButtonTooltip = true;
            ShowSaveButtonTooltip = true;
            ShowDeleteButtonTooltip = true;
        }
    }
}

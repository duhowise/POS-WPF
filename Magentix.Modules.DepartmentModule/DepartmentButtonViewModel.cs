﻿using Magentix.Presentation.Common;
using Magentix.Presentation.Common.Commands;
using Magentix.Presentation.Services;

namespace Magentix.Modules.DepartmentModule
{
    public class DepartmentButtonViewModel : ObservableObject
    {
        private readonly IApplicationState _applicationState;
        private readonly DepartmentSelectorViewModel _parentViewModel;

        public DepartmentButtonViewModel(DepartmentSelectorViewModel parentViewModel,
            IApplicationState applicationState)
        {
            _parentViewModel = parentViewModel;
            _applicationState = applicationState;
            DepartmentSelectionCommand = new CaptionCommand<string>("Select", OnSelectDepartment);
        }

        private void OnSelectDepartment(string obj)
        {
            _parentViewModel.UpdateSelectedDepartment(DepartmentId);
        }

        public ICaptionCommand DepartmentSelectionCommand { get; set; }

        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string ButtonColor { get { return _applicationState.CurrentDepartment != null && _applicationState.CurrentDepartment.Id == DepartmentId ? "#2D8CF0" : "#39D9EA"; } }

        public void Refresh()
        {
            RaisePropertyChanged(() => ButtonColor);
        }
    }
}

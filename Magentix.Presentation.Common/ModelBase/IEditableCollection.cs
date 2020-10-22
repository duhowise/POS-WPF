﻿using Magentix.Presentation.Common.Commands;

namespace Magentix.Presentation.Common.ModelBase
{
    public interface IEditableCollection
    {
        ICaptionCommand AddItemCommand { get; set; }
        ICaptionCommand EditItemCommand { get; set; }
        ICaptionCommand DeleteItemCommand { get; set; }
    }
}

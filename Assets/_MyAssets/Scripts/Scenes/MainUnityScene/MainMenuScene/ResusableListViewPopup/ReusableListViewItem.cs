using UnityEngine;
using UnityEngine.UI;
using MyClasses.UI;

public class ReusableListViewItem : MyUGUIReusableListItem
{
    [SerializeField]
    private Text _txtIndex;

    public override void OnReload()
    {
        ReusableListViewItemModel model = (ReusableListViewItemModel)Model;
        _txtIndex.text = this.Index + ": " + (model != null ? model.Letter : string.Empty);
    }
}

public class ReusableListViewItemModel
{
    public string Letter;
}
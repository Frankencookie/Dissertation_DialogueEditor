using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using System.Linq;

public class DialogueGraphView : GraphView
{
    public DialogueGraphView()
    {
        //Load Stylesheet to make everything look cool
        styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));

        //Add controls
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        //Add grid background
        GridBackground grid = new GridBackground();
        Insert(0, grid);
    }
}

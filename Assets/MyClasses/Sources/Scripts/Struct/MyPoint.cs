/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyPoint (version 1.0)
 */

[System.Serializable]
public struct MyPoint
{
    public readonly static MyPoint Zero = new MyPoint(0, 0);

    public int Row;
    public int Col;

    public MyPoint(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public override string ToString()
    {
        return string.Format("({0}, {1}", Row, Col);
    }
}
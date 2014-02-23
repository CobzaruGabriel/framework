﻿using System;
using UnityEngine;

namespace Mercraft.Models.Primitives
{
        /// <summary>
        /// Summary description for Polygon.
        /// </summary>
        public class Polygon
        {

            private Vector2[] m_aVertices;

            public Vector2 this[int index]
            {
                set
                {
                    m_aVertices[index] = value;
                }
                get
                {
                    return m_aVertices[index];
                }
            }

            public Polygon()
            {

            }

            public Polygon(Vector2[] points)
            {
                int nNumOfPoitns = points.Length;
                try
                {
                    if (nNumOfPoitns < 3)
                    {
                       throw new ArgumentException("At least 3 points required!");
                    }
                    else
                    {
                        m_aVertices = new Vector2[nNumOfPoitns];
                        for (int i = 0; i < nNumOfPoitns; i++)
                        {
                            m_aVertices[i] = points[i];
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.WriteLine(
                        e.Message + e.StackTrace);
                }
            }

            /***********************************
             From a given point, get its vertex index.
             If the given point is not a polygon vertex, 
             it will return -1 
             ***********************************/
            public int VertexIndex(Vector2 vertex)
            {
                int nIndex = -1;

                int nNumPts = m_aVertices.Length;
                for (int i = 0; i < nNumPts; i++) //each vertex
                {
                    if (m_aVertices[i].SamePoint(vertex))
                        nIndex = i;
                }
                return nIndex;
            }

            /***********************************
             From a given vertex, get its previous vertex point.
             If the given point is the first one, 
             it will return  the last vertex;
             If the given point is not a polygon vertex, 
             it will return null; 
             ***********************************/
            public Vector2? PreviousPoint(Vector2 vertex)
            {
                int nIndex;

                nIndex = VertexIndex(vertex);
                if (nIndex == -1)
                    return null;
                else //a valid vertex
                {
                    if (nIndex == 0) //the first vertex
                    {
                        int nPoints = m_aVertices.Length;
                        return m_aVertices[nPoints - 1];
                    }
                    else //not the first vertex
                        return m_aVertices[nIndex - 1];
                }
            }

            /***************************************
                 From a given vertex, get its next vertex point.
                 If the given point is the last one, 
                 it will return  the first vertex;
                 If the given point is not a polygon vertex, 
                 it will return null; 
            ***************************************/
            public Vector2? NextPoint(Vector2 vertex)
            {
                Vector2 nextPt = new Vector2();

                int nIndex;
                nIndex = VertexIndex(vertex);
                if (nIndex == -1)
                    return null;
                else //a valid vertex
                {
                    int nNumOfPt = m_aVertices.Length;
                    if (nIndex == nNumOfPt - 1) //the last vertex
                    {
                        return m_aVertices[0];
                    }
                    else //not the last vertex
                        return m_aVertices[nIndex + 1];
                }
            }


            /******************************************
            To calculate the polygon's area

            Good for polygon with holes, but the vertices make the 
            hole  should be in different direction with bounding 
            polygon.
		
            Restriction: the polygon is not self intersecting
            ref: www.swin.edu.au/astronomy/pbourke/
                geometry/polyarea/
            *******************************************/
            public double PolygonArea()
            {
                double dblArea = 0;
                int nNumOfVertices = m_aVertices.Length;

                int j;
                for (int i = 0; i < nNumOfVertices; i++)
                {
                    j = (i + 1) % nNumOfVertices;
                    dblArea += m_aVertices[i].x * m_aVertices[j].y;
                    dblArea -= (m_aVertices[i].y * m_aVertices[j].x);
                }

                dblArea = dblArea / 2;
                return Math.Abs(dblArea);
            }

            /******************************************
            To calculate the area of polygon made by given points 

            Good for polygon with holes, but the vertices make the 
            hole  should be in different direction with bounding 
            polygon.
		
            Restriction: the polygon is not self intersecting
            ref: www.swin.edu.au/astronomy/pbourke/
                geometry/polyarea/

            As polygon in different direction, the result coulb be
            in different sign:
            If dblArea>0 : polygon in clock wise to the user 
            If dblArea<0: polygon in count clock wise to the user 		
            *******************************************/
            public static double PolygonArea(Vector2[] points)
            {
                double dblArea = 0;
                int nNumOfPts = points.Length;

                int j;
                for (int i = 0; i < nNumOfPts; i++)
                {
                    j = (i + 1) % nNumOfPts;
                    dblArea += points[i].x * points[j].y;
                    dblArea -= (points[i].y * points[j].x);
                }

                dblArea = dblArea / 2;
                return dblArea;
            }

            /***********************************************
                To check a vertex concave point or a convex point
                -----------------------------------------------------------
                The out polygon is in count clock-wise direction
            ************************************************/
            public VertexType PolygonVertexType(Vector2 vertex)
            {
                VertexType vertexType = VertexType.ErrorPoint;

                if (PolygonVertex(vertex))
                {
                    Vector2 pti = vertex;
                    Vector2 ptj = PreviousPoint(vertex).Value;
                    Vector2 ptk = NextPoint(vertex).Value;

                    double dArea = PolygonArea(new Vector2[] { ptj, pti, ptk });

                    if (dArea < 0)
                        vertexType = VertexType.ConvexPoint;
                    else if (dArea > 0)
                        vertexType = VertexType.ConcavePoint;
                }
                return vertexType;
            }


            /*********************************************
            To check the Line of vertex1, vertex2 is a Diagonal or not
  
            To be a diagonal, Line vertex1-vertex2 has no intersection 
            with polygon lines.
		
            If it is a diagonal, return true;
            If it is not a diagonal, return false;
            reference: www.swin.edu.au/astronomy/pbourke
            /geometry/lineline2d
            *********************************************/
            public bool Diagonal(Vector2 vertex1, Vector2 vertex2)
            {
                bool bDiagonal = false;
                int nNumOfVertices = m_aVertices.Length;
                int j = 0;
                for (int i = 0; i < nNumOfVertices; i++) //each point
                {
                    bDiagonal = true;
                    j = (i + 1) % nNumOfVertices;  //next point of i

                    //Diagonal line:
                    double x1 = vertex1.x;
                    double y1 = vertex1.y;
                    double x2 = vertex1.x;
                    double y2 = vertex1.y;

                    //Polygon line:
                    double x3 = m_aVertices[i].x;
                    double y3 = m_aVertices[i].y;
                    double x4 = m_aVertices[j].x;
                    double y4 = m_aVertices[j].y;

                    double de = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);
                    double ub = -1;

                    if (Math.Abs(de - 0) > ConstantValue.SmallValue)  //lines are not parallel
                        ub = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / de;

                    if ((ub > 0) && (ub < 1))
                    {
                        bDiagonal = false;
                    }
                }
                return bDiagonal;
            }


            /*************************************************
            To check FaVertices make a convex polygon or 
            concave polygon

            Restriction: the polygon is not self intersecting
            Ref: www.swin.edu.au/astronomy/pbourke
            /geometry/clockwise/index.html
            ********************************************/
            public PolygonType GetPolygonType()
            {
                int nNumOfVertices = m_aVertices.Length;
                bool bSignChanged = false;
                int nCount = 0;
                int j = 0, k = 0;

                for (int i = 0; i < nNumOfVertices; i++)
                {
                    j = (i + 1) % nNumOfVertices; //j:=i+1;
                    k = (i + 2) % nNumOfVertices; //k:=i+2;

                    double crossProduct = (m_aVertices[j].x - m_aVertices[i].x)
                        * (m_aVertices[k].y - m_aVertices[j].y);
                    crossProduct = crossProduct - (
                        (m_aVertices[j].y - m_aVertices[i].y)
                        * (m_aVertices[k].x - m_aVertices[j].x)
                        );

                    //change the value of nCount
                    if ((crossProduct > 0) && (nCount == 0))
                        nCount = 1;
                    else if ((crossProduct < 0) && (nCount == 0))
                        nCount = -1;

                    if (((nCount == 1) && (crossProduct < 0))
                        || ((nCount == -1) && (crossProduct > 0)))
                        bSignChanged = true;
                }

                if (bSignChanged)
                    return PolygonType.Concave;
                else
                    return PolygonType.Convex;
            }

            /***************************************************
            Check a Vertex is a principal vertex or not
            ref. www-cgrl.cs.mcgill.ca/~godfried/teaching/
            cg-projects/97/Ian/glossay.html
  
            PrincipalVertex: a vertex pi of polygon P is a principal vertex if the
            diagonal pi-1, pi+1 intersects the boundary of P only at pi-1 and pi+1.
            *********************************************************/
            public bool PrincipalVertex(Vector2 vertex)
            {
                bool bPrincipal = false;
                if (PolygonVertex(vertex)) //valid vertex
                {
                    Vector2 pt1 = PreviousPoint(vertex).Value;
                    Vector2 pt2 = NextPoint(vertex).Value;

                    if (Diagonal(pt1, pt2))
                        bPrincipal = true;
                }
                return bPrincipal;
            }

            /*********************************************
            To check whether a given point is a Polygon Vertex
            **********************************************/
            public bool PolygonVertex(Vector2 point)
            {
                bool bVertex = false;
                int nIndex = VertexIndex(point);

                if ((nIndex >= 0) && (nIndex <= m_aVertices.Length - 1))
                    bVertex = true;

                return bVertex;
            }

            /*****************************************************
            To reverse polygon vertices to different direction:
            clock-wise <------->count-clock-wise
            ******************************************************/
            public void ReverseVerticesDirection()
            {
                int nVertices = m_aVertices.Length;
                Vector2[] aTempPts = new Vector2[nVertices];

                for (int i = 0; i < nVertices; i++)
                    aTempPts[i] = m_aVertices[i];

                for (int i = 0; i < nVertices; i++)
                    m_aVertices[i] = aTempPts[nVertices - 1 - i];
            }

            /*****************************************
            To check vertices make a clock-wise polygon or
            count clockwise polygon

            Restriction: the polygon is not self intersecting
            Ref: www.swin.edu.au/astronomy/pbourke/
            geometry/clockwise/index.html
            *****************************************/
            public PolygonDirection VerticesDirection()
            {
                int nCount = 0, j = 0, k = 0;
                int nVertices = m_aVertices.Length;

                for (int i = 0; i < nVertices; i++)
                {
                    j = (i + 1) % nVertices; //j:=i+1;
                    k = (i + 2) % nVertices; //k:=i+2;

                    double crossProduct = (m_aVertices[j].x - m_aVertices[i].x)
                        * (m_aVertices[k].y - m_aVertices[j].y);
                    crossProduct = crossProduct - (
                        (m_aVertices[j].y - m_aVertices[i].y)
                        * (m_aVertices[k].x - m_aVertices[j].x)
                        );

                    if (crossProduct > 0)
                        nCount++;
                    else
                        nCount--;
                }

                if (nCount < 0)
                    return PolygonDirection.Count_Clockwise;
                else if (nCount > 0)
                    return PolygonDirection.Clockwise;
                else
                    return PolygonDirection.Unknown;
            }


            /*****************************************
            To check given points make a clock-wise polygon or
            count clockwise polygon

            Restriction: the polygon is not self intersecting
            *****************************************/
            public static PolygonDirection PointsDirection(
                Vector2[] points)
            {
                int nCount = 0, j = 0, k = 0;
                int nPoints = points.Length;

                if (nPoints < 3)
                    return PolygonDirection.Unknown;

                for (int i = 0; i < nPoints; i++)
                {
                    j = (i + 1) % nPoints; //j:=i+1;
                    k = (i + 2) % nPoints; //k:=i+2;

                    double crossProduct = (points[j].x - points[i].x)
                        * (points[k].y - points[j].y);
                    crossProduct = crossProduct - (
                        (points[j].y - points[i].y)
                        * (points[k].x - points[j].x)
                        );

                    if (crossProduct > 0)
                        nCount++;
                    else
                        nCount--;
                }

                if (nCount < 0)
                    return PolygonDirection.Count_Clockwise;
                else if (nCount > 0)
                    return PolygonDirection.Clockwise;
                else
                    return PolygonDirection.Unknown;
            }

            /*****************************************************
            To reverse points to different direction (order) :
            ******************************************************/
            public static void ReversePointsDirection(
                Vector2[] points)
            {
                int nVertices = points.Length;
                Vector2[] aTempPts = new Vector2[nVertices];

                for (int i = 0; i < nVertices; i++)
                    aTempPts[i] = points[i];

                for (int i = 0; i < nVertices; i++)
                    points[i] = aTempPts[nVertices - 1 - i];
            }

        }
    

}
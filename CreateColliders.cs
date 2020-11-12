using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateColliders : MonoBehaviour
{
    //Take all pixels of a province read through them to find borders then create a collider with those as vertices
    public Color[] Imagedata;
    public List<int> pixels;
    public List<Vector2> coordList;
    public List<Vector2> vertices;
    public BoxCollider2D coll;
    public SpriteRenderer SpriteRenderer;
    public Color ID;
    public PolygonCollider2D Poly;
    public bool Ran;

    public int Width { get { return SpriteRenderer.sprite.texture.width; } }
    public int Height { get { return SpriteRenderer.sprite.texture.height; } }
    // Start is called before the first frame update
    void Start()
    {
        // Get renderer you want to probe
        SpriteRenderer = GetComponent<SpriteRenderer>();

        // extract color data
        Imagedata = SpriteRenderer.sprite.texture.GetPixels();

        //
        coll = GetComponent<BoxCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Ran == false)
        {
            //runs all methods once
            Ran = true;
            FindPixels();
            findPixelCoords();
            GetVertices();
            orientVertices();
            FormCollider();
        }
        
    }
    public void FindPixels()
    {
        for (int i = 0; i < Imagedata.Length; i++)
        {
            if (Imagedata[i] == ID)
            {
                pixels.Add(i);
            }
        }
    }
    public void findPixelCoords()
    {
        Debug.Log("finished adding pixels");
        for(int i=0; i < pixels.Count; i++)
        {
            int y = (int)pixels[i] / Width;//find the y value by the number of times the width can be divided without decimals
            int x = pixels[i] % Width;//find x value by finding the remainder
            coordList.Add(new Vector2(x, y));
        }
    }
    public void GetVertices()
    {
        List<Vector2> Xlist = new List<Vector2>();
        //find every coord with the same y value
        for (int v = 0; v < Width; v++)
        {
            Xlist.Clear();
            for(int h=0; h < Height; h++)
            {
                if(coordList.Contains(new Vector2(h, v)))
                {
                    Xlist.Add(new Vector2(h, v));
                }
            }
            //take the highest and lowest x values
            Vector2 min = new Vector2();
            Vector2 max = new Vector2();
            for (int r = 0; r<Xlist.Count; r++)
            {
                if (Xlist[r].x > max.x)
                {
                    max = Xlist[r];
                }
                if (Xlist[r].x < min.x)
                {
                    min = Xlist[r];
                }
            }
            //add them to the vertices array
            if (vertices.Contains(max)==false)
            {
                vertices.Add(max);
            }
            if (vertices.Contains(min)==false)
            {
                vertices.Add(min);
            }
        }

    }
    public void orientVertices()
    {
        Debug.Log("made it to orient");
        //set vertices to world space
        Vector3 transformPos = GetComponent<Transform>().position;
        Vector3 origin = new Vector3(transformPos.x - (coll.size.x / 2), transformPos.y - (coll.size.y / 2));
        for(int i= 0; i<vertices.Count; i ++)
        {
            vertices[i] = new Vector3(vertices[i].x/10f, vertices[i].y/10f) + origin;
        }
    }
    public void FormCollider()
    {
        //set vertices to polygon collider vertices
        for (int i= 0; i<vertices.Count; i++)
        {
            Poly.points[i] = vertices[i];
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EmptyCharacter.Utils
{
    public static class Utils
    {
        public const int sortingOrderDefault = 1000;
        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector2 localPosition= default(Vector2),int fontSize= 40, Color? color = null, TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center,int sortingOrder = sortingOrderDefault)
        {
            if(color == null) color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
        }

        public static TextMesh CreateWorldText(Transform parent, string text, Vector2 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("WorldText", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();    
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;   
            textMesh.color = color;
            textMesh.fontSize = fontSize;   
            textMesh.GetComponent<MeshRenderer>().sortingOrder= sortingOrder;

            return textMesh; 
        }
        private static Transform cachedCanvasTransform; 
        public static Transform GetCanvasTransform()
        {
            if(cachedCanvasTransform == null)
            {
                Canvas canvas = MonoBehaviour.FindAnyObjectByType<Canvas>();
                if (canvas != null)
                {
                    cachedCanvasTransform = canvas.transform;   

                }
            }
            return cachedCanvasTransform;
        }

        public static GameObject CreateWorldSprite(string name, Sprite sprite, Vector2 position, Vector2 localScale, int sortingOrder, Color color)
        {
            return CreateWorldSprite(null, name, sprite, position, localScale, sortingOrder, color);
        }

        public static GameObject CreateWorldSprite(Transform parent, string name, Sprite sprite, Vector2 localPosition, Vector2 localScale, int sortingOrder, Color color)
        {
            GameObject gameObject = new GameObject("BlueGrid",typeof(SpriteRenderer));
            Transform transform = gameObject.transform; 
            transform.SetParent(parent,false);
            transform.localPosition = localPosition;
            transform.localScale = localScale; 
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite; 
            spriteRenderer.sortingOrder = sortingOrder;
            spriteRenderer.color = color;
            gameObject.layer = LayerMask.NameToLayer("BlueGrid");
            gameObject.AddComponent<BoxCollider2D>();
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.3f, 1.3f);
            return gameObject;

        }
        public static GameObject CreatePathSprite(string name, Sprite sprite, Vector2 position, Vector2 localScale, int sortingOrder, Color color)
        {
            return CreatePathSprite(null,name,sprite,position,localScale, sortingOrder, color);
        }
        public static GameObject CreatePathSprite(Transform parent, string name, Sprite sprite, Vector2 localPosition, Vector2 localScale, int sortingOrder, Color color)
        {
            GameObject gameObject = new GameObject("Path", typeof(SpriteRenderer));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.localScale = localScale;
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = sortingOrder;
            spriteRenderer.color = color;
            gameObject.layer = LayerMask.NameToLayer("PathGrid");
            gameObject.AddComponent<BoxCollider2D>();
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.3f, 1.3f);
            return gameObject;

        }
        public static GameObject CreateTestSprite(string name, Sprite sprite, Vector2 position, Vector2 localScale, int sortingOrder, Color color)
        {
            return CreateTestSprite(null, name, sprite, position, localScale, sortingOrder, color);
        }
        public static GameObject CreateTestSprite(Transform parent, string name, Sprite sprite, Vector2 localPosition, Vector2 localScale, int sortingOrder, Color color)
        {
            GameObject gameObject = new GameObject("Test", typeof(SpriteRenderer));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.localScale = localScale;
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = sortingOrder;
            spriteRenderer.color = color;
            gameObject.layer = LayerMask.NameToLayer("PathGrid");
            gameObject.AddComponent<BoxCollider2D>();
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.3f, 1.3f);
            return gameObject;

        }
        public static GameObject CreateGridColliders(string name, Vector2 position, Vector2 localScale)
        {
            GameObject gameObject = new GameObject("Grid", typeof(SpriteRenderer));
            Transform transform = gameObject.transform;
            transform.localPosition = position;
            transform.localScale = localScale;
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            gameObject.layer = LayerMask.NameToLayer("Grid");
            gameObject.AddComponent<BoxCollider2D>();
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.3f, 1.3f);
            return gameObject;
        }
        
        public static GameObject CreateWorldBoundaries(string name, Sprite sprite, Vector2 position, Vector2 localScale, int sortingOrder, Color color)
        {
            return CreateWorldBoundaries(null, name, sprite , position, localScale, sortingOrder, color);
        }
        public static GameObject CreateWorldBoundaries(Transform parent, string name, Sprite sprite, Vector2 localPosition, Vector2 localScale, int sortingOrder, Color color)
        {
            GameObject gameObject = new GameObject("Boundary", typeof(SpriteRenderer));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.localScale = localScale;
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            gameObject.tag = "Wall";
            spriteRenderer.sortingOrder = sortingOrder;
            spriteRenderer.color = color;
            gameObject.layer = LayerMask.NameToLayer("Boundary");
            gameObject.AddComponent<Rigidbody2D>();
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            gameObject.AddComponent<BoxCollider2D>();
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.3f, 1.3f);
            return gameObject;
        }
        public static Vector2 GetMouseWorldPosition()
        {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec; 
        }
        public static Vector2 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }
        public static Vector2 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }
        public static Vector2 GetMouseWorldPositionWithZ(Vector2 screenPosition, Camera worldCamera)
        {
            Vector2 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
    }
}


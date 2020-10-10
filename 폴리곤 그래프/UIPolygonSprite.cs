//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------
// 2016-09-01 성혁 미완성

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Sprite is a textured element in the UI hierarchy.
/// </summary>

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/DC/NGUI Polygon")]
public class UIPolygonSprite : UIBasicSprite
{
	// Cached and saved values
	[HideInInspector][SerializeField] UIAtlas mAtlas;
	[HideInInspector][SerializeField] string mSpriteName;
    [HideInInspector][SerializeField] string mSideSpriteName;
    [HideInInspector][SerializeField] bool mFillCenter = true;
    [HideInInspector][SerializeField] int mPolygonSides = 3;
    [HideInInspector][SerializeField] float[] mVerticesDistances = new float[3];
    [HideInInspector][SerializeField] Vector2[] mVectorSides = new Vector2[3];
    [HideInInspector][SerializeField] UISprite[] mSpriteSides = new UISprite[3];
    [HideInInspector][SerializeField] int mThickness = 5;
    [HideInInspector][SerializeField] bool mTest = false;

    [System.NonSerialized] protected UISpriteData mSprite;
	[System.NonSerialized] bool mSpriteSet = false;

	/// <summary>
	/// Retrieve the material used by the font.
	/// </summary>

	public override Material material { get { return (mAtlas != null) ? mAtlas.spriteMaterial : null; } }

	/// <summary>
	/// Atlas used by this widget.
	/// </summary>
 
	public UIAtlas atlas
	{
		get
		{
			return mAtlas;
		}
		set
		{
			if (mAtlas != value)
			{
				RemoveFromPanel();

				mAtlas = value;
				mSpriteSet = false;
				mSprite = null;

				// Automatically choose the first sprite
				if (string.IsNullOrEmpty(mSpriteName))
				{
					if (mAtlas != null && mAtlas.spriteList.Count > 0)
					{
						SetAtlasSprite(mAtlas.spriteList[0]);
						mSpriteName = mSprite.name;
					}
				}

				// Re-link the sprite
				if (!string.IsNullOrEmpty(mSpriteName))
				{
					string sprite = mSpriteName;
					mSpriteName = "";
					spriteName = sprite;
					MarkAsChanged();
				}
			}
		}
	}

	/// <summary>
	/// Sprite within the atlas used to draw this widget.
	/// </summary>
 
	public string spriteName
	{
		get
		{
			return mSpriteName;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				// If the sprite name hasn't been set yet, no need to do anything
				if (string.IsNullOrEmpty(mSpriteName)) return;

				// Clear the sprite name and the sprite reference
				mSpriteName = "";
				mSprite = null;
				mChanged = true;
				mSpriteSet = false;
			}
			else if (mSpriteName != value)
			{
				// If the sprite name changes, the sprite reference should also be updated
				mSpriteName = value;
				mSprite = null;
				mChanged = true;
				mSpriteSet = false;
			}
		}
	}

	/// <summary>
	/// Is there a valid sprite to work with?
	/// </summary>

	public bool isValid { get { return GetAtlasSprite() != null; } }

	/// <summary>
	/// Whether the center part of the sprite will be filled or not. Turn it off if you want only to borders to show up.
	/// </summary>

	[System.Obsolete("Use 'centerType' instead")]
	public bool fillCenter
	{
		get
		{
			return centerType != AdvancedType.Invisible;
		}
		set
		{
			if (value != (centerType != AdvancedType.Invisible))
			{
				centerType = value ? AdvancedType.Sliced : AdvancedType.Invisible;
				MarkAsChanged();
			}
		}
	}

	/// <summary>
	/// Whether a gradient will be applied.
	/// </summary>

	public bool applyGradient
	{
		get
		{
			return mApplyGradient;
		}
		set
		{
			if (mApplyGradient != value)
			{
				mApplyGradient = value;
				MarkAsChanged();
			}
		}
	}

	/// <summary>
	/// Top gradient color.
	/// </summary>

	public Color gradientTop
	{
		get
		{
			return mGradientTop;
		}
		set
		{
			if (mGradientTop != value)
			{
				mGradientTop = value;
				if (mApplyGradient) MarkAsChanged();
			}
		}
	}

	/// <summary>
	/// Bottom gradient color.
	/// </summary>

	public Color gradientBottom
	{
		get
		{
			return mGradientBottom;
		}
		set
		{
			if (mGradientBottom != value)
			{
				mGradientBottom = value;
				if (mApplyGradient) MarkAsChanged();
			}
		}
	}

	/// <summary>
	/// Sliced sprites generally have a border. X = left, Y = bottom, Z = right, W = top.
	/// </summary>

	public override Vector4 border
	{
		get
		{
			UISpriteData sp = GetAtlasSprite();
			if (sp == null) return base.border;
			return new Vector4(sp.borderLeft, sp.borderBottom, sp.borderRight, sp.borderTop);
		}
	}

	/// <summary>
	/// Size of the pixel -- used for drawing.
	/// </summary>

	override public float pixelSize { get { return mAtlas != null ? mAtlas.pixelSize : 1f; } }

	/// <summary>
	/// Minimum allowed width for this widget.
	/// </summary>

	override public int minWidth
	{
		get
		{
			if (type == Type.Sliced || type == Type.Advanced)
			{
				float ps = pixelSize;
				Vector4 b = border * pixelSize;
				int min = Mathf.RoundToInt(b.x + b.z);

				UISpriteData sp = GetAtlasSprite();
				if (sp != null) min += Mathf.RoundToInt(ps * (sp.paddingLeft + sp.paddingRight));

				return Mathf.Max(base.minWidth, ((min & 1) == 1) ? min + 1 : min);
			}
			return base.minWidth;
		}
	}

	/// <summary>
	/// Minimum allowed height for this widget.
	/// </summary>

	override public int minHeight
	{
		get
		{
			if (type == Type.Sliced || type == Type.Advanced)
			{
				float ps = pixelSize;
				Vector4 b = border * pixelSize;
				int min = Mathf.RoundToInt(b.y + b.w);

				UISpriteData sp = GetAtlasSprite();
				if (sp != null) min += Mathf.RoundToInt(ps * (sp.paddingTop + sp.paddingBottom));

				return Mathf.Max(base.minHeight, ((min & 1) == 1) ? min + 1 : min);
			}
			return base.minHeight;
		}
	}

	/// <summary>
	/// Sprite's dimensions used for drawing. X = left, Y = bottom, Z = right, W = top.
	/// This function automatically adds 1 pixel on the edge if the sprite's dimensions are not even.
	/// It's used to achieve pixel-perfect sprites even when an odd dimension sprite happens to be centered.
	/// </summary>

	public override Vector4 drawingDimensions
	{
		get
		{
			Vector2 offset = pivotOffset;

			float x0 = -offset.x * mWidth;
			float y0 = -offset.y * mHeight;
			float x1 = x0 + mWidth;
			float y1 = y0 + mHeight;

			if (GetAtlasSprite() != null && mType != Type.Tiled)
			{
				int padLeft = mSprite.paddingLeft;
				int padBottom = mSprite.paddingBottom;
				int padRight = mSprite.paddingRight;
				int padTop = mSprite.paddingTop;

				if (mType != Type.Simple)
				{
					float ps = pixelSize;

					if (ps != 1f)
					{
						padLeft = Mathf.RoundToInt(ps * padLeft);
						padBottom = Mathf.RoundToInt(ps * padBottom);
						padRight = Mathf.RoundToInt(ps * padRight);
						padTop = Mathf.RoundToInt(ps * padTop);
					}
				}

				int w = mSprite.width + padLeft + padRight;
				int h = mSprite.height + padBottom + padTop;
				float px = 1f;
				float py = 1f;

				if (w > 0 && h > 0 && (mType == Type.Simple || mType == Type.Filled))
				{
					if ((w & 1) != 0) ++padRight;
					if ((h & 1) != 0) ++padTop;

					px = (1f / w) * mWidth;
					py = (1f / h) * mHeight;
				}

				if (mFlip == Flip.Horizontally || mFlip == Flip.Both)
				{
					x0 += padRight * px;
					x1 -= padLeft * px;
				}
				else
				{
					x0 += padLeft * px;
					x1 -= padRight * px;
				}

				if (mFlip == Flip.Vertically || mFlip == Flip.Both)
				{
					y0 += padTop * py;
					y1 -= padBottom * py;
				}
				else
				{
					y0 += padBottom * py;
					y1 -= padTop * py;
				}
			}

			Vector4 br = (mAtlas != null) ? border * pixelSize : Vector4.zero;

			float fw = br.x + br.z;
			float fh = br.y + br.w;

			float vx = Mathf.Lerp(x0, x1 - fw, mDrawRegion.x);
			float vy = Mathf.Lerp(y0, y1 - fh, mDrawRegion.y);
			float vz = Mathf.Lerp(x0 + fw, x1, mDrawRegion.z);
			float vw = Mathf.Lerp(y0 + fh, y1, mDrawRegion.w);

			return new Vector4(vx, vy, vz, vw);
		}
	}

	/// <summary>
	/// Whether the texture is using a premultiplied alpha material.
	/// </summary>

	public override bool premultipliedAlpha { get { return (mAtlas != null) && mAtlas.premultipliedAlpha; } }

    public float[] VerticesDistance
    {
        get
        {
            MarkAsChanged();
            return mVerticesDistances;
        }
    }

    public void SetDistance(int sideCount, float value = 1.0f)
    {
        mPolygonSides = sideCount;

        if (mVerticesDistances.Length != sideCount + 1)
        {
            mVerticesDistances = new float[sideCount + 1];
            for (int i = 0; i < sideCount + 1; i++)
            {
                mVerticesDistances[i] = value;
            }
        }

        MarkAsChanged();
    }

    public void InitDistance(float value = 1.0f)
    {
        for (int i = 0; i < mVerticesDistances.Length; i++)
        {
            mVerticesDistances[i] = value;
        }

        MarkAsChanged();
    }

    public void SetDisdis(int index, float value)
    {
        mVerticesDistances[index] = value;
        if (width * mVerticesDistances[index] < minWidth)
        {
            mVerticesDistances[index] = minWidth / (float)width;
        }
        
        if (index == mPolygonSides)
            mVerticesDistances[index] = mVerticesDistances[0];

        MarkAsChanged();
    }

	/// <summary>
	/// Retrieve the atlas sprite referenced by the spriteName field.
	/// </summary>

	public UISpriteData GetAtlasSprite ()
	{
		if (!mSpriteSet) mSprite = null;

		if (mSprite == null && mAtlas != null)
		{
			if (!string.IsNullOrEmpty(mSpriteName))
			{
				UISpriteData sp = mAtlas.GetSprite(mSpriteName);
				if (sp == null) return null;
				SetAtlasSprite(sp);
			}

			if (mSprite == null && mAtlas.spriteList.Count > 0)
			{
				UISpriteData sp = mAtlas.spriteList[0];
				if (sp == null) return null;
				SetAtlasSprite(sp);

				if (mSprite == null)
				{
					Debug.LogError(mAtlas.name + " seems to have a null sprite!");
					return null;
				}
				mSpriteName = mSprite.name;
			}
		}
		return mSprite;
	}

	/// <summary>
	/// Set the atlas sprite directly.
	/// </summary>

	protected void SetAtlasSprite (UISpriteData sp)
	{
		mChanged = true;
		mSpriteSet = true;

		if (sp != null)
		{
			mSprite = sp;
			mSpriteName = mSprite.name;
		}
		else
		{
			mSpriteName = (mSprite != null) ? mSprite.name : "";
			mSprite = sp;
		}
	}

	/// <summary>
	/// Adjust the scale of the widget to make it pixel-perfect.
	/// </summary>

	public override void MakePixelPerfect ()
	{
		if (!isValid) return;
		base.MakePixelPerfect();
		if (mType == Type.Tiled) return;

		UISpriteData sp = GetAtlasSprite();
		if (sp == null) return;

		Texture tex = mainTexture;
		if (tex == null) return;

		if (mType == Type.Simple || mType == Type.Filled || !sp.hasBorder)
		{
			if (tex != null)
			{
				int x = Mathf.RoundToInt(pixelSize * (sp.width + sp.paddingLeft + sp.paddingRight));
				int y = Mathf.RoundToInt(pixelSize * (sp.height + sp.paddingTop + sp.paddingBottom));
				
				if ((x & 1) == 1) ++x;
				if ((y & 1) == 1) ++y;

				width = x;
				height = y;
			}
		}
	}

	/// <summary>
	/// Auto-upgrade.
	/// </summary>

	protected override void OnInit ()
	{
//		if (!mFillCenter)
//		{
//			mFillCenter = true;
//			centerType = AdvancedType.Invisible;
//#if UNITY_EDITOR
//			NGUITools.SetDirty(this);
//#endif
//		}
		base.OnInit();
	}

	/// <summary>
	/// Update the UV coordinates.
	/// </summary>

	protected override void OnUpdate ()
	{
		base.OnUpdate();

		if (mChanged || !mSpriteSet)
		{
			mSpriteSet = true;
			mSprite = null;
			mChanged = true;
		}
	}

	/// <summary>
	/// Virtual function called by the UIPanel that fills the buffers.
	/// </summary>

	public override void OnFill (List<Vector3> verts, List<Vector2> uvs, List<Color> cols)
	{
		Texture tex = mainTexture;
		if (tex == null) return;

		if (mSprite == null) mSprite = atlas.GetSprite(spriteName);
		if (mSprite == null) return;

		Rect outer = new Rect(mSprite.x, mSprite.y, mSprite.width, mSprite.height);
		Rect inner = new Rect(mSprite.x + mSprite.borderLeft, mSprite.y + mSprite.borderTop,
			mSprite.width - mSprite.borderLeft - mSprite.borderRight,
			mSprite.height - mSprite.borderBottom - mSprite.borderTop);

		outer = NGUIMath.ConvertToTexCoords(outer, tex.width, tex.height);
		inner = NGUIMath.ConvertToTexCoords(inner, tex.width, tex.height);

		int offset = verts.Count;
        PolyFill(verts, uvs, cols, outer, inner);

		if (onPostFill != null)
			onPostFill(this, offset, verts, uvs, cols);
	}

    protected void PolyFill(List<Vector3> verts, List<Vector2> uvs, List<Color> cols, Rect outer, Rect inner)
    {
        Vector4 v = drawingDimensions;
        Vector4 u = new Vector4(outer.xMin, outer.yMin, outer.xMax, outer.yMax);
        Vector4 uu = new Vector4(inner.xMin, inner.yMin, inner.xMax, inner.yMax);
        Color gc = drawingColor;
        Color lc = gc.GammaToLinearSpace();

        Vector2 prevX = Vector2.zero;
        Vector2 prevY = Vector2.zero;
        Vector2 pos0;
        Vector2 pos1;
        Vector2 pos2;
        Vector2 pos3;
        Vector2 temppos0;
        Vector2 temppos1;
        Vector2 temppos2;
        Vector2 temppos3;
        Vector4 br = border * pixelSize;
        float prevC = 0;
        float prevS = 0;
        float prevInline = 0;
        float degrees = 360f / mPolygonSides;
        int vertices = mPolygonSides + 1;
        mVectorSides = new Vector2[vertices];

        // last vertex is also the first!
        for (int i = 0; i < vertices; i++)
        {
            // 중심 x 너비 x factor 
            float outline = width * 0.5f * mVerticesDistances[i];
            float inline = width * 0.5f * mVerticesDistances[i] - (br.w * mVerticesDistances[i]);

            float rad = Mathf.Deg2Rad * (i * degrees);
            float c = Mathf.Cos(rad);
            float s = Mathf.Sin(rad);
            pos0 = prevX;
            pos1 = new Vector2(outline * c, outline * s);

            mVectorSides[i] = pos1 + new Vector2(c * -3, s * -3);

            if (mTest)
            {
                if (i != mPolygonSides && mSpriteSides[i] != null)
                {
                    mSpriteSides[i].transform.localPosition = mVectorSides[i];
                }
            }

            // 가운데를 채울경우에는 사각형 이미지에서 uv를 계산해서 따온다
            // 채우지 않을 때에는 선마다 이미지를 그려준다 (사각형으로)
            if (mFillCenter)
            {
                pos2 = Vector2.zero;
                pos3 = Vector2.zero;

                // 아틀라스상의 uv최소값 + (최대값-최소값) * ((cos값 + 1) / 2 * factor) (uv좌표와 삼각함수좌표 일치시키는부분)
                uvs.Add(new Vector2(u.x + ((u.z - u.x) * (prevC + 1) * 0.5f), u.y + ((u.w - u.y) * ((prevS + 1) * 0.5f))));
                uvs.Add(new Vector2(u.x + ((u.z - u.x) * (c + 1) * 0.5f), u.y + ((u.w - u.y) * ((s + 1) * 0.5f))));
                uvs.Add(new Vector2((u.x + u.z) * 0.5f, (u.y + u.w) * 0.5f));
                uvs.Add(new Vector2((u.x + u.z) * 0.5f, (u.y + u.w) * 0.5f));

                // 이전좌표로부터 반시계방향으로 그림 -> 시계방향으로 바꾸자...
                verts.Add(new Vector3(pos0.x, pos0.y));
                verts.Add(new Vector3(pos1.x, pos1.y));
                verts.Add(new Vector3(pos2.x, pos2.y));
                verts.Add(new Vector3(pos3.x, pos3.y));
            }
            else
            {
                //pos2 = new Vector2(inline * c, inline * s);
                //pos3 = prevY;

                if (mTest)
                {
                    // 아 짜다가 머리꺠질거같다..
                    // 하드코딩으로 일단짜보자..
                    // 총 4번그려야함 오른쪽모서리부터 그려보자
                    //temppos0 = pos0; // 맨끝꼭지점
                    //temppos1 = pos0 - new Vector2(br.z * c, s); // 맨끝꼭지점에서 - 삼각함수x보더만큼 빼준다
                    //temppos2 = pos1;
                    //temppos3 = pos1;

                    //verts.Add(new Vector3(temppos0.x, temppos0.y));
                    //verts.Add(new Vector3(temppos1.x, temppos1.y));
                    //verts.Add(new Vector3(temppos2.x, temppos2.y));
                    //verts.Add(new Vector3(temppos3.x, temppos3.y));

                    //uvs.Add(new Vector2(u.z, u.w));
                    //uvs.Add(new Vector2(u.x, u.w));
                    //uvs.Add(new Vector2((u.x + u.z) * 0.5f, (u.y + u.w) * 0.5f));
                    //uvs.Add(new Vector2((u.x + u.z) * 0.5f, (u.y + u.w) * 0.5f));

                    //for (int x = 0; x < 4; ++x)
                    //{
                    //    temppos0 = pos0;
                    //    temppos1 = pos0 - new Vector2(br.z * c, br.x * s);
                    //}
                    // x는 중심값으로  y도 어찌어찌구하자... 사실나도모름 
                    pos2 = new Vector2(inline * c, inline * s);
                    pos3 = new Vector2(prevInline * prevC, prevInline * prevS);

                    // 위 ----------------------
                    verts.Add(new Vector3(pos0.x, pos0.y));
                    verts.Add(new Vector3(pos1.x, pos1.y));
                    verts.Add(new Vector3(pos2.x, pos2.y));
                    verts.Add(new Vector3(pos3.x, pos3.y));

                    // 0번은 xmax ymax 1번은 xmin ymax 2, 3 중심
                    uvs.Add(new Vector2(u.z, u.w));
                    uvs.Add(new Vector2(u.x, u.w));
                    uvs.Add(new Vector2(uu.x, uu.w));
                    uvs.Add(new Vector2(uu.z, uu.w));

                    temppos0 = pos3;
                    temppos1 = pos2;
                    temppos2 = Vector2.zero;
                    temppos3 = Vector2.zero;

                    // 아래 -----------------
                    verts.Add(new Vector3(temppos0.x, temppos0.y));
                    verts.Add(new Vector3(temppos1.x, temppos1.y));
                    verts.Add(new Vector3(temppos2.x, temppos2.y));
                    verts.Add(new Vector3(temppos3.x, temppos3.y));

                    // 0번은 xmax ymax 1번은 xmin ymax 2, 3 중심
                    uvs.Add(new Vector2(uu.z, uu.w));
                    uvs.Add(new Vector2(uu.x, uu.w));
                    uvs.Add(new Vector2((u.x + u.z) * 0.5f, (u.y + u.w) * 0.5f));
                    uvs.Add(new Vector2((u.x + u.z) * 0.5f, (u.y + u.w) * 0.5f));

                    //// 오른쪽 ---------------------
                    //verts.Add(new Vector3(pos0.x, pos0.y));
                    //verts.Add(new Vector3(pos1.x, pos1.y));
                    //verts.Add(new Vector3(pos2.x, pos2.y));
                    //verts.Add(new Vector3(pos3.x, pos3.y));

                    //// 0번은 xmax ymax 1번은 xmin ymax 2, 3 중심
                    //uvs.Add(new Vector2(u.z, u.w));
                    //uvs.Add(new Vector2(uu.z, u.w));
                    //uvs.Add(new Vector2(uu.z, uu.w));
                    //uvs.Add(new Vector2(uu.z, uu.w));

                    //// 위-------------------
                    //verts.Add(new Vector3(pos0.x, pos0.y));
                    //verts.Add(new Vector3(pos1.x, pos1.y));
                    //verts.Add(new Vector3(pos2.x, pos2.y));
                    //verts.Add(new Vector3(pos3.x, pos3.y));

                    //// 0번은 xmax ymax 1번은 xmin ymax 2, 3 중심
                    //uvs.Add(new Vector2(uu.z, u.w));
                    //uvs.Add(new Vector2(uu.x, u.w));
                    //uvs.Add(new Vector2(uu.z, uu.w));
                    //uvs.Add(new Vector2(uu.x, uu.w));

                    ////왼쪽-----------------------
                    //verts.Add(new Vector3(pos0.x, pos0.y));
                    //verts.Add(new Vector3(pos1.x, pos1.y));
                    //verts.Add(new Vector3(pos2.x, pos2.y));
                    //verts.Add(new Vector3(pos3.x, pos3.y));

                    //// 0번은 xmax ymax 1번은 xmin ymax 2, 3 중심
                    //uvs.Add(new Vector2(uu.x, u.w));
                    //uvs.Add(new Vector2(u.x, u.w));
                    //uvs.Add(new Vector2(uu.x, uu.w));
                    //uvs.Add(new Vector2(uu.x, uu.w));

                    //// 중앙 ------------------
                    //verts.Add(new Vector3(pos0.x, pos0.y));
                    //verts.Add(new Vector3(pos1.x, pos1.y));
                    //verts.Add(new Vector3(pos2.x, pos2.y));
                    //verts.Add(new Vector3(pos3.x, pos3.y));

                    //// 0번은 xmax ymax 1번은 xmin ymax 2, 3 중심
                    //uvs.Add(new Vector2(uu.z, uu.w));
                    //uvs.Add(new Vector2(uu.x, uu.w));
                    //uvs.Add(new Vector2((u.x + u.z) * 0.5f, (u.y + u.w) * 0.5f));
                    //uvs.Add(new Vector2((u.x + u.z) * 0.5f, (u.y + u.w) * 0.5f));

                }
                else
                {
                    pos2 = Vector2.zero;
                    pos3 = Vector2.zero;

                    //uvs.Add(new Vector2(u.z, u.w));
                    //uvs.Add(new Vector2(u.x, u.w));
                    //uvs.Add(new Vector2(u.x, u.y));
                    //uvs.Add(new Vector2(u.z, u.y));

                    // 이전좌표로부터 반시계방향으로 그림 -> 시계방향으로 바꾸자...
                    verts.Add(new Vector3(pos0.x, pos0.y));
                    verts.Add(new Vector3(pos1.x, pos1.y));
                    verts.Add(new Vector3(pos2.x, pos2.y));
                    verts.Add(new Vector3(pos3.x, pos3.y));

                    // 0번은 xmax ymax 1번은 xmin ymax 2, 3 중심
                    uvs.Add(new Vector2(u.z, u.w));
                    uvs.Add(new Vector2(u.x, u.w));
                    uvs.Add(new Vector2((u.x + u.z) * 0.5f, (u.y + u.w) * 0.5f));
                    uvs.Add(new Vector2((u.x + u.z) * 0.5f, (u.y + u.w) * 0.5f));
                }
            }
            prevX = pos1;
            prevY = pos2;
            prevInline = inline;

            // 이전좌표로부터 반시계방향으로 그림 -> 시계방향으로 바꾸자...
            //verts.Add(new Vector3(pos0.x, pos0.y));
            //verts.Add(new Vector3(pos1.x, pos1.y));
            //verts.Add(new Vector3(pos2.x, pos2.y));
            //verts.Add(new Vector3(pos3.x, pos3.y));

            //uvs.Add(new Vector2(u.x + ((u.z - u.x) * (prevC + 1) * 0.5f), u.y + ((u.w - u.y) * (prevS + 1) * 0.5f)));
            //uvs.Add(new Vector2(u.x + ((u.z - u.x) * (c + 1) * 0.5f), u.y + ((u.w - u.y) * (s + 1) * 0.5f)));
            //uvs.Add(new Vector2((u.x + u.z) * 0.5f, (u.y + u.w) * 0.5f));
            //uvs.Add(new Vector2((u.x + u.z) * 0.5f, (u.y + u.w) * 0.5f));

            prevC = c;
            prevS = s;

            cols.Add(lc);
            cols.Add(lc);
            cols.Add(lc);
            cols.Add(lc);

            if (mTest)
            {
                cols.Add(lc);
                cols.Add(lc);
                cols.Add(lc);
                cols.Add(lc);
            }
        }
    }

    protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs)
    {
        UIVertex[] vbo = new UIVertex[4];
        for (int i = 0; i < vertices.Length; i++)
        {
            var vert = UIVertex.simpleVert;
            vert.color = color;
            vert.position = vertices[i];
            vert.uv0 = uvs[i];
            vbo[i] = vert;
        }
        return vbo;
    }
}

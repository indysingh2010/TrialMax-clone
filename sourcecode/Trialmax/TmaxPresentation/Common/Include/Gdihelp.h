//==============================================================================
//
// File Name:	gdihelp.h
//
// Description:	This file contains the declarations of a set of helper classes
//				used to manage GDI resources.
//
// Author:		Kenneth Moore
//
// Copyright	
//
//==============================================================================
//	Date		Revision    Description
//	05-31-01	1.00		Original Release
//==============================================================================
#if !defined(__GDIHELP_H__)
#define __GDIHELP_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	This class simplifies management of a memory DC. Automatically copies the
//	source dc when created and blits the memory dc back into the source when
//	it gets destroyed.
class CGHMemDC : public CDC
{
	private:

	public:

		CGHMemDC(CDC* pdc, LPRECT lpRect = NULL,  BOOL bCopy = FALSE) : CDC()
		{
			ASSERT(pdc != NULL);

			m_pdc = pdc;
			m_pOldBitmap = 0;
	
			m_bMemory = !pdc->IsPrinting();

			if(m_bMemory)
			{
				if(lpRect)
					m_Rect = *lpRect;
				else
					pdc->GetClipBox(&m_Rect);

				//	Create the memory dc
				CreateCompatibleDC(pdc);

				//	Select bitmap into the dc
				m_Bitmap.CreateCompatibleBitmap(pdc, m_Rect.Width(), m_Rect.Height());
				m_pOldBitmap = SelectObject(&m_Bitmap);

				//	Set the origin to the lop left corner
				SetWindowOrg(m_Rect.left, m_Rect.top);

				//	Should we copy the contents of the source dc?
				if(bCopy)
					BitBlt(m_Rect.left, m_Rect.top, m_Rect.Width(), m_Rect.Height(),
						   m_pdc, m_Rect.left, m_Rect.top, SRCCOPY);
			}
			else
			{
				//	Copy the members we need for printing
				m_bPrinting = pdc->m_bPrinting;
				m_hDC = pdc->m_hDC;
				m_hAttribDC = pdc->m_hAttribDC;
			}
		}

		virtual	~CGHMemDC()
		{
			if(m_bMemory) 
			{	 
				//	Copy the bitmap from the memory dc back into the source dc
				m_pdc->BitBlt(m_Rect.left, m_Rect.top, m_Rect.Width(), m_Rect.Height(),
							  this, m_Rect.left, m_Rect.top, SRCCOPY);

				//	Clean up
				SelectObject(m_pOldBitmap);
			} 
			else 
			{
				//	This prevents us from improperly deleting handles associated with
				//	the source dc
				m_hDC = NULL;
				m_hAttribDC = NULL;
			}
		}

		// Allow indirect referencing
		CGHMemDC* operator->(){return this;}
		operator CGHMemDC*()  {return this;}

	protected:

		CBitmap				m_Bitmap;      
		CBitmap*			m_pOldBitmap;  
		CDC*				m_pdc;         
		CRect				m_Rect;
		BOOL				m_bMemory; 
};

//	Custom font class
class CGHFont : public CFont
{
	private:

	public:
	
							CGHFont(){}
							CGHFont(CDC* pdc, int size, LPCTSTR lpFace = NULL, 
									BOOL bBold = 0, BOOL bItalic = 0, 
									BOOL bUnderline = 0, BOOL bFixed = 0,
									BOOL bHiQuality = 0, int iRotation = 0)
									{
										VERIFY(Create(pdc, size, lpFace, bBold, 
													  bItalic, bUnderline,
													  bFixed, bHiQuality, 
													  iRotation));
									}
		
		BOOL				Create(CDC* pdc, int size, LPCTSTR lpFace = NULL,
								   BOOL bBold = 0, BOOL bItalic = 0, 
								   BOOL bUnderline = 0, BOOL bFixed = 0, 
								   BOOL bHiQuality = 0, int iRotation = 0)
							{
								ASSERT(pdc);
								CSize Size(0, MulDiv(size, pdc->GetDeviceCaps(LOGPIXELSY), 72));
								pdc->DPtoLP(&Size);
								return CreateFont(-abs(Size.cy), 0, 10*iRotation, 0,
												  bBold ? FW_BOLD : FW_NORMAL, BYTE(bItalic),
												  BYTE(bUnderline), 0, ANSI_CHARSET,
												  OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS,
												  BYTE(bHiQuality ? PROOF_QUALITY : DEFAULT_QUALITY),
												  BYTE(bFixed ? FF_MODERN|FIXED_PITCH :
												  FF_SWISS|VARIABLE_PITCH),
											      lpFace &&*lpFace ? lpFace : NULL);
							}
};

//	Base class for GDI selection classes. Used to store the associated dc.
class CGHSelect
{
	private:

		// Disable copying
		int operator = (const CGHSelect &);
		CGHSelect(const CGHSelect &);

	public:

							CGHSelect(CDC* pdc):m_pdc(pdc){ ASSERT(m_pdc); }
		virtual			   ~CGHSelect(){};

	protected:

		CDC* const			m_pdc;

};

//	Helper class for selecting stock objects into the dc
class CGHSelStock : public CGHSelect
{
	private:

		CGdiObject*			m_pGdiOld;

	public:

							CGHSelStock(CDC* pdc):CGHSelect(pdc), m_pGdiOld(NULL){}
							CGHSelStock(CDC* pdc, int iIndex):CGHSelect(pdc), m_pGdiOld(NULL){ Select(iIndex); }
		virtual			   ~CGHSelStock(){ Restore(); }

		void				Select(int iIndex)
							{
								CGdiObject* pOld = m_pdc->SelectStockObject(iIndex); ASSERT(pOld);
								if(m_pGdiOld == NULL) m_pGdiOld = pOld;
							}

		void				Restore()
							{
								if(m_pGdiOld) VERIFY(m_pdc->SelectObject(m_pGdiOld));
								m_pGdiOld = NULL;
							}

		CGdiObject*			GetOldGdi() const { return m_pGdiOld; }
};

//	Helper class for selecting pens into the dc
class CGHSelPen : public CGHSelect
{
	private:

		CPen				m_Pen;
		CPen*				m_pOldPen;

	public:

							CGHSelPen(CDC* pdc):CGHSelect(pdc), m_pOldPen(NULL){}
							CGHSelPen(CDC* pdc, CPen* pPen)
									  :CGHSelect(pdc), m_pOldPen(NULL)
									  { Select(pPen); }
							CGHSelPen(CDC* pdc, COLORREF crColor, int iStyle = PS_SOLID, int iWidth = 0)
									  :CGHSelect(pdc), m_pOldPen(NULL)
									  { Select(crColor, iStyle, iWidth); }

		virtual			   ~CGHSelPen(){ Restore(); }

		void				Select(CPen* pPen)
							{
								ASSERT(pPen);
								ASSERT(pPen != &m_Pen);
								CPen* pOld = m_pdc->SelectObject(pPen); ASSERT(pOld);
								if(m_pOldPen == NULL) m_pOldPen = pOld;
								m_Pen.DeleteObject();
							}

		void				Select(COLORREF crColor, int iStyle = PS_SOLID, int iWidth = 0)
							{
								if(m_pOldPen) Select(m_pOldPen);
								VERIFY(m_Pen.CreatePen(iStyle, iWidth, crColor));
								VERIFY(m_pOldPen = m_pdc->SelectObject(&m_Pen));
							}

		void				Restore()
							{
								if(m_pOldPen) VERIFY(m_pdc->SelectObject(m_pOldPen));
								m_pOldPen = NULL;
								m_Pen.DeleteObject();
							}

		CPen*				GetOldPen() const { return m_pOldPen; }
};

//	Helper class for selecting bitmaps into the dc
class CGHSelBitmap	: public CGHSelect
{
	private:

		CBitmap				m_Bitmap;
		CBitmap*			m_pOldBitmap;

	public:
	
							CGHSelBitmap(CDC* pdc)
									     :CGHSelect(pdc), m_pOldBitmap(NULL){}
							CGHSelBitmap(CDC* pSelect, CDC* pCompatible, int iWidth, int iHeight)
									     :CGHSelect(pSelect), m_pOldBitmap(NULL)
									     { Select(pCompatible, iWidth, iHeight); }
							CGHSelBitmap(CDC* pdc, CBitmap* pBitmap)
									     :CGHSelect(pdc), m_pOldBitmap(NULL)
									     { Select(pBitmap); }

		virtual			   ~CGHSelBitmap(){ Restore(); }

		void				Select(CBitmap* pBitmap)
							{
								ASSERT(pBitmap);
								ASSERT(pBitmap != &m_Bitmap);
								CBitmap* pOld = m_pdc->SelectObject(pBitmap);	ASSERT(pOld);
								if(!m_pOldBitmap) m_pOldBitmap = pOld;
								m_Bitmap.DeleteObject();
							}

		void				Select(CDC* pCompatible, int iWidth, int iHeight)
							{
								ASSERT(pCompatible);
								if(m_pOldBitmap) Select(m_pOldBitmap);
								VERIFY(m_Bitmap.CreateCompatibleBitmap(pCompatible, iWidth, iHeight));
								VERIFY(m_pOldBitmap = m_pdc->SelectObject(&m_Bitmap));
							}

		void				Restore()
							{
								if(m_pOldBitmap) VERIFY(m_pdc->SelectObject(m_pOldBitmap));
								m_pOldBitmap = NULL;
								m_Bitmap.DeleteObject();
							}

		CBitmap*			GetOldBitmap() const { return m_pOldBitmap; }
};

//	Helper class for selecting bitmaps into the dc
class CGHSelBrush  : public CGHSelect
{
	private:

		CBrush				m_Brush;
		CBrush*				m_pOldBrush;

	public:
	
							CGHSelBrush(CDC* pdc)
										:CGHSelect(pdc), m_pOldBrush(NULL){}
							CGHSelBrush(CDC* pdc, COLORREF crColor)
										:CGHSelect(pdc), m_pOldBrush(NULL)
										{ Select(crColor); }
							CGHSelBrush(CDC* pdc, int iIndex, COLORREF crColor)
										:CGHSelect(pdc), m_pOldBrush(NULL)
										{ Select(iIndex, crColor); }
							CGHSelBrush(CDC* pdc, CBitmap* pBitmap)
										:CGHSelect(pdc), m_pOldBrush(NULL)
										{ Select(pBitmap); }
							CGHSelBrush(CDC* pdc, HGLOBAL hPackedDib, UINT uUsage)
										:CGHSelect(pdc), m_pOldBrush(NULL)
										{ Select(hPackedDib, uUsage); }
							CGHSelBrush(CDC* pdc, CBrush* pBrush)
										:CGHSelect(pdc), m_pOldBrush(NULL)
										{ Select(pBrush); }

		virtual			   ~CGHSelBrush(){ Restore(); }

		void				Select(CBrush* pBrush)
							{
								ASSERT(pBrush);
								ASSERT(pBrush != &m_Brush);
								CBrush* pOld = m_pdc->SelectObject(pBrush); ASSERT(pOld);
								if(!m_pOldBrush) m_pOldBrush=pOld;
								m_Brush.DeleteObject();
							}

							// Solid brush
		void				Select(COLORREF crColor)
							{
								if(m_pOldBrush) Select(m_pOldBrush);
								VERIFY(m_Brush.CreateSolidBrush(crColor));
								VERIFY(m_pOldBrush = m_pdc->SelectObject(&m_Brush));
							}
							// Hatch brush
		void				Select(int iIndex, COLORREF crColor)
							{
								if(m_pOldBrush) Select(m_pOldBrush);
								VERIFY(m_Brush.CreateHatchBrush(iIndex, crColor));
								VERIFY(m_pOldBrush = m_pdc->SelectObject(&m_Brush));
							}
							// Pattern brush
		void				Select(CBitmap* pBitmap)
							{
								ASSERT(pBitmap);
								if(m_pOldBrush) Select(m_pOldBrush);
								VERIFY(m_Brush.CreatePatternBrush(pBitmap));
								VERIFY(m_pOldBrush = m_pdc->SelectObject(&m_Brush));
							}
							// DIB Pattern brush
		void				Select(HGLOBAL hPackedDib, UINT uUsage)
							{
								if(m_pOldBrush) Select(m_pOldBrush);
								VERIFY(m_Brush.CreateDIBPatternBrush(hPackedDib, uUsage));
								VERIFY(m_pOldBrush=m_pdc->SelectObject(&m_Brush));
							}

		void				Restore()
							{
								if(m_pOldBrush) VERIFY(m_pdc->SelectObject(m_pOldBrush));
								m_pOldBrush = NULL;
								m_Brush.DeleteObject();
							}

		CBrush*				GetOldBrush() const { return m_pOldBrush; }
};

//	Helper class for selecting fonts into the dc
class CGHSelFont  : public CGHSelect
{
	CGHFont					m_Font;
	CFont*					m_pOldFont;

	public:

							CGHSelFont(CDC* pdc)
									   :CGHSelect(pdc), m_pOldFont(NULL){}
							CGHSelFont(CDC* pdc, int iSize, LPCTSTR lpFace = NULL, 
									   BOOL bBold = 0, BOOL bItalic = 0, 
									   BOOL bUnderline = 0, BOOL bFixed = 0,
									   BOOL bHiQuality = 0, int iRotation = 0)
									   :CGHSelect(pdc), m_pOldFont(NULL)
									   {
											Select(iSize, lpFace, bBold, bItalic, 
												   bUnderline, bFixed, bHiQuality, 
												   iRotation);
									   }
							CGHSelFont(CDC* pdc, CFont* pFont)
									   :CGHSelect(pdc), m_pOldFont(NULL)
									   { Select(pFont); }
							CGHSelFont(CDC* pdc, const LOGFONT* lpLogFont)
									   :CGHSelect(pdc), m_pOldFont(NULL)
									   { Select(lpLogFont); }

		virtual			   ~CGHSelFont(){ Restore(); }

		void				Select(CFont* pFont)
							{
								ASSERT(pFont);
								ASSERT(pFont != &m_Font);
								CFont* pOld = m_pdc->SelectObject(pFont); ASSERT(pOld);
								if(!m_pOldFont) m_pOldFont = pOld;
								m_Font.DeleteObject();
							}

		void				Select(int iSize, LPCTSTR lpFace = NULL, 
								   BOOL bBold = 0, BOOL bItalic = 0, 
								   BOOL bUnderline = 0, BOOL bFixed = 0,
								   BOOL bHiQuality = 0, int iRotation = 0)
							{
								if(m_pOldFont) Select(m_pOldFont);
								VERIFY(m_Font.Create(m_pdc, iSize, lpFace, bBold, bItalic,
		                               bUnderline, bFixed, bHiQuality, iRotation));
								VERIFY(m_pOldFont = m_pdc->SelectObject(&m_Font));
							}

		void				Select(const LOGFONT* lpLogFont)
							{
								if(m_pOldFont) Select(m_pOldFont);
								VERIFY(m_Font.CreateFontIndirect(lpLogFont));
								VERIFY(m_pOldFont = m_pdc->SelectObject(&m_Font));
							}

		void				Restore()
							{
								if(m_pOldFont) VERIFY(m_pdc->SelectObject(m_pOldFont));
								m_pOldFont = NULL;
								m_Font.DeleteObject();
							}

		CFont*				GetOldFont() const	{ return m_pOldFont; }
};

//	Helper class for selecting palettes into the dc
class CGHSelPalette  : public CGHSelect
{
	private:

		CPalette*			m_pOldPalette;
		BOOL				m_bForceBackground;
		BOOL				m_bRealizePalette;

	public:

							CGHSelPalette(CDC* pdc)
										  :CGHSelect(pdc), m_pOldPalette(NULL){}
							CGHSelPalette(CDC* pdc, CPalette* pPalette,
										  BOOL bForceBackground = FALSE, 
										  BOOL bRealize = TRUE)
										  :CGHSelect(pdc), m_pOldPalette(NULL)
										  { 
											Select(pPalette, bForceBackground, 
												   bRealize); 
										  }

		virtual			   ~CGHSelPalette(){ Restore(); }

		UINT				Select(CPalette* pPalette,
								   BOOL bForceBackground = FALSE, 
								   BOOL bRealize = TRUE)
							{
								ASSERT(pPalette);
								ASSERT(m_pdc->GetDeviceCaps(RASTERCAPS)&RC_PALETTE);
								CPalette* pOld=m_pdc->SelectPalette(pPalette, bForceBackground);
								ASSERT(pOld);
								if(!m_pOldPalette) m_pOldPalette=pOld;
								m_bForceBackground = bForceBackground;
								m_bRealizePalette = bRealize;
								return bRealize ? m_pdc->RealizePalette() : 0;
							}

		void				ChangeRestoreFlags(BOOL bForceBackground, BOOL bRealize)
							{
								m_bForceBackground = bForceBackground;
								m_bRealizePalette = bRealize;
							}
	
		void				Restore()
							{
								if(!m_pOldPalette)
									return;

								VERIFY(m_pdc->SelectPalette(m_pOldPalette, m_bForceBackground));
								if(m_bRealizePalette) 
									m_pdc->RealizePalette();
								m_pOldPalette = NULL;
							}

		CPalette*			GetOldPalette() const	{ return m_pOldPalette; }
};

//	Helper class for setting background color mode
class CGHSelBkColor : public CGHSelect
{
	private:

		COLORREF			m_OldBkColor;

	public:

							CGHSelBkColor(CDC* pdc)
										  :CGHSelect(pdc), m_OldBkColor(CLR_INVALID)
										  { m_OldBkColor = m_pdc->GetBkColor(); }
							CGHSelBkColor(CDC* pdc, COLORREF crColor)
										  :CGHSelect(pdc), m_OldBkColor(CLR_INVALID)
										  { Select(crColor); }

		virtual			   ~CGHSelBkColor(){ Restore(); }

		void				Select(COLORREF crColor)
							{
								ASSERT(crColor != CLR_INVALID);
								int iOld = m_pdc->SetBkColor(crColor);	ASSERT(iOld != CLR_INVALID);
								if(m_OldBkColor == CLR_INVALID) m_OldBkColor = iOld;
							}

		void				Restore()
							{
								if(m_OldBkColor == CLR_INVALID) return;
								VERIFY(m_pdc->SetBkColor(m_OldBkColor) != CLR_INVALID);
								m_OldBkColor = CLR_INVALID;
							}

		COLORREF			GetOldBkColor() const { return m_OldBkColor; }
};

//	Helper class for setting ROP mode
class CGHSelROP2 : public CGHSelect
{
	private:

		int					m_iOldRop;

	public:

							CGHSelROP2(CDC* pdc)
									   :CGHSelect(pdc), m_iOldRop(0){}
							CGHSelROP2(CDC* pdc, int iDrawMode)
									   :CGHSelect(pdc), m_iOldRop(0)
									   { Select(iDrawMode); }

		virtual			   ~CGHSelROP2(){ Restore(); }

		void				Select(int iDrawMode)
							{
								int iOld = m_pdc->SetROP2(iDrawMode); ASSERT(iOld);
								if(!m_iOldRop) m_iOldRop = iOld;
							}

		void				Restore()
							{
								if(m_iOldRop) VERIFY(m_pdc->SetROP2(m_iOldRop));
								m_iOldRop = 0;
							}

		int					GetOldRop() const { return m_iOldRop; }
};

//	Helper class for setting background mode
class CGHSelBkMode : public CGHSelect
{
	private:

		int					m_iOldBkMode;

	public:

							CGHSelBkMode(CDC* pdc)
										 :CGHSelect(pdc), m_iOldBkMode(0){}
							CGHSelBkMode(CDC* pdc, int iBkMode)
										 :CGHSelect(pdc), m_iOldBkMode(0)
										 { Select(iBkMode); }

		virtual			   ~CGHSelBkMode(){ Restore(); }

		void				Select(int iBkMode)
							{
								int iOld = m_pdc->SetBkMode(iBkMode); ASSERT(iOld);
								if(!m_iOldBkMode) m_iOldBkMode = iOld ;
							}

		void				Restore()
							{
								if(m_iOldBkMode) VERIFY(m_pdc->SetBkMode(m_iOldBkMode));
								m_iOldBkMode = 0;
							}

		int					GetOldBkMode() const { return m_iOldBkMode; }
};

//	Helper class for setting map mode
class CGHSelMapMode : public CGHSelect
{
	private:

		int					m_iOldMapMode;

	public:

							CGHSelMapMode(CDC* pdc)
										 :CGHSelect(pdc), m_iOldMapMode(0){}
							CGHSelMapMode(CDC* pdc, int iMapMode)
										 :CGHSelect(pdc), m_iOldMapMode(0)
										 { Select(iMapMode); }

		virtual			   ~CGHSelMapMode(){ Restore(); }

		void				Select(int iMapMode)
							{
								int iOld = m_pdc->SetMapMode(iMapMode); ASSERT(iOld);
								if(!m_iOldMapMode) m_iOldMapMode = iOld ;
							}

		void				Restore()
							{
								if(m_iOldMapMode) VERIFY(m_pdc->SetMapMode(m_iOldMapMode));
								m_iOldMapMode = 0;
							}

		int					GetOldMapMode() const { return m_iOldMapMode; }
};

//	Helper class for setting text alignment
class CGHSelTextAlign : public CGHSelect
{
	private:

		UINT				m_uOldAlign;

	public:

							CGHSelTextAlign(CDC* pdc)
										    :CGHSelect(pdc), m_uOldAlign(GDI_ERROR)
											{ m_uOldAlign = m_pdc->GetTextAlign(); }
							CGHSelTextAlign(CDC* pdc, UINT uAlign)
											:CGHSelect(pdc), m_uOldAlign(GDI_ERROR)
											{ Select(uAlign); }

		virtual			   ~CGHSelTextAlign(){ Restore(); }

		void				Select(UINT uAlign)
							{
								ASSERT(uAlign != GDI_ERROR);
								UINT uOld = m_pdc->SetTextAlign(uAlign); ASSERT(uOld != GDI_ERROR);
								if(m_uOldAlign == GDI_ERROR) m_uOldAlign = uOld;
							}

		void				Restore()
							{
								if(m_uOldAlign == GDI_ERROR) return;
								VERIFY(m_pdc->SetTextAlign(m_uOldAlign) != GDI_ERROR);
								m_uOldAlign = GDI_ERROR;
							}

		UINT				GetOldTextAlign() const { return m_uOldAlign; }
};

//	Helper class for setting text color
class CGHSelTextColor : public CGHSelect
{
	private:

		COLORREF				m_crOldColor;

	public:

								CGHSelTextColor(CDC* pdc)
												:CGHSelect(pdc), m_crOldColor(CLR_INVALID)
												{ m_crOldColor = m_pdc->GetTextColor(); }
								CGHSelTextColor(CDC* pdc, COLORREF crColor)
												:CGHSelect(pdc), m_crOldColor(CLR_INVALID)
												{ Select(crColor); }

		virtual				   ~CGHSelTextColor(){ Restore(); }

		void					Select(COLORREF crColor)
								{
									ASSERT(crColor != CLR_INVALID);
									int iOld = m_pdc->SetTextColor(crColor); ASSERT(iOld != CLR_INVALID);
									if(m_crOldColor == CLR_INVALID) m_crOldColor = iOld;
								}

		void					Restore()
								{
									if(m_crOldColor == CLR_INVALID) return;
									VERIFY(m_pdc->SetTextColor(m_crOldColor) != CLR_INVALID);
									m_crOldColor = CLR_INVALID;
								}

		COLORREF				GetOldTextColor() const { return m_crOldColor; }
};

//	Class for saving the state of the dc
class CGHSaveDC : public CGHSelect
{
	private:

		int					m_iSaved;

	public:

							CGHSaveDC(CDC* pdc) : CGHSelect(pdc)
									 { VERIFY(m_iSaved = m_pdc->SaveDC()); }

		virtual			   ~CGHSaveDC(){ Restore(); }

		void				Restore()
							{
								if(m_iSaved) VERIFY(m_pdc->RestoreDC(m_iSaved));
								m_iSaved = 0;
							}

		int					GetSaved() const { return m_iSaved; }
};

#endif // !defined(__GDIHELP_H__)

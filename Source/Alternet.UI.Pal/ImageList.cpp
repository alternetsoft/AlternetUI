#include "ImageList.h"

namespace Alternet::UI
{
    ImageList::ImageList(): _imageList(new wxImageList(16, 16, true, 1))
    {
    }
    
    ImageList::~ImageList()
    {
        delete _imageList;
        _imageList = nullptr;
    }

    Size ImageList::GetPixelImageSize()
    {
        return _imageList->GetSize();
    }
    
    void ImageList::SetPixelImageSize(const Size& value)
    {
        // todo: recreate image list.
        wxASSERT(false);
        throw 0;
    }
    
    void ImageList::AddImage(Image* image)
    {
        wxBitmap bitmap(*(image->GetImage()));
        _imageList->Add(bitmap);
    }
    
    wxImageList* ImageList::GetImageList()
    {
        return _imageList;
    }
}

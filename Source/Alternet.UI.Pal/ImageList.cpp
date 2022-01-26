#include "ImageList.h"

namespace Alternet::UI
{
    ImageList::ImageList() :
        _pixelImageSize(fromDip(16, nullptr), fromDip(16, nullptr))
    {
        CreateImageList();
    }
    
    ImageList::~ImageList()
    {
        DestroyImageList();
    }

    Int32Size ImageList::GetPixelImageSize()
    {
        return _imageList->GetSize();
    }

    void ImageList::SetPixelImageSize(const Int32Size& value)
    {
        if (_pixelImageSize == value)
            return;

        _pixelImageSize = value;
        RecreateImageList();
    }

    SizeF ImageList::GetImageSize()
    {
        return toDip(_imageList->GetSize(), nullptr);
    }

    void ImageList::SetImageSize(const SizeF& value)
    {
        SetPixelImageSize(fromDip(value, nullptr));
    }

    void ImageList::AddImage(Image* image)
    {
        AddImageCore(image->GetImage());
    }

    void ImageList::AddImageCore(const wxImage& image)
    {
        wxASSERT(_imageList);
        if (_imageList == nullptr)
            throw 0;

        auto finalImage = image;
        auto targetSize = _imageList->GetSize();

        if (finalImage.GetSize() != targetSize)
            finalImage = finalImage.Scale(targetSize.x, targetSize.y, wxIMAGE_QUALITY_BILINEAR);

        wxBitmap bitmap(finalImage);
        _imageList->Add(bitmap);
    }

    wxImageList* ImageList::GetImageList()
    {
        return _imageList;
    }
    
    void ImageList::CreateImageList()
    {
        wxASSERT(_imageList == nullptr);

        _imageList = new wxImageList(_pixelImageSize.Width, _pixelImageSize.Height, true, 1);
    }

    void ImageList::DestroyImageList()
    {
        if (_imageList == nullptr)
            return;

        delete _imageList;
        _imageList = nullptr;
    }

    void ImageList::RecreateImageList()
    {
        std::vector<wxBitmap> bitmapsToRestore;

        if (_imageList != nullptr)
        {
            for (int i = 0; _imageList->GetImageCount(); i++)
                bitmapsToRestore.push_back(_imageList->GetBitmap(i));
        }
        
        DestroyImageList();
        CreateImageList();

        for (auto bitmap : bitmapsToRestore)
            AddImageCore(bitmap.ConvertToImage());
    }
}

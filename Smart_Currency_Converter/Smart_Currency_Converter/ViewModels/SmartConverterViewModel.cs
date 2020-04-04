﻿using System.IO;
using Plugin.Media;
using Xamarin.Forms;
using System.ComponentModel;
using Plugin.Media.Abstractions;
using Model.Smart_Currency_Converter;

namespace ViewModel.SmartConverter
{
    public class SmartConverterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ImageProcessingHelper imageProcessing;
        private ImageSource image;
        
        public Command TakePhoto { get; }

        public SmartConverterViewModel()
        {
            imageProcessing = new ImageProcessingHelper();
            TakePhoto = new Command(CameraButtonClickedAsync);
        }

        public ImageSource ImageDisplay
        {
            get => image;
            set
            {
                image = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ImageDisplay)));
            }
        }

        private async void CameraButtonClickedAsync()
        {
            MediaFile photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions() { });
            byte[] imageByteArray = ConvertImageToByte(photo);
            ImageDisplay = ImageSource.FromStream(() => new MemoryStream(imageByteArray));

            imageProcessing.PostImageForAnalysis(imageByteArray);
        }

        //TODO: Add try & catch
        // Check for stuff: if (imageByteArray == null || imageByteArray.Length == 0)
        private byte[] ConvertImageToByte(MediaFile photo)
        {
            byte[] imageArray = null;

            using (MemoryStream memory = new MemoryStream()) {

                Stream stream = photo.GetStream();
                stream.CopyTo(memory);
                imageArray = memory.ToArray();
            }

            return imageArray;
        }
    }
}
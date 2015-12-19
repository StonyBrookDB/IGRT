A = importdata('ct_slice.txt');

my_Size=size(A);
orientation=512;
%for cbct and ct both
isocx=-15.86;
isocy=-129.49;

%for cbct
x_origin=-15.9;
y_origin=-129;

pixel_width=0.69;
pixel_height=0.69;
central_x=orientation/2;
central_y=orientation/2;

isocenter_imx=floor(central_x+(isocx/pixel_width))
isocenter_imy=floor(central_y+(isocy/pixel_height))

%steps_in_x=floor(abs((isocx-x_origin)/pixel_width));
%steps_in_y=floor(abs((isocy-y_origin)/pixel_height));

for n = 1:my_Size(2)
    if A(n) > 254
        A(n) = 254; 
    end
end
%for ct
B=reshape(A,512,512);

%y,x





%B(isocenter_imy,:)=254;
%B(:,isocenter_imx)=254;

M=mat2gray(B);
BW1 = edge(M,'sobel');
figure, imshow(BW1);



M = cat(3, M, M, M);
blueplane=M(:, :, 3);
blueplane(isocenter_imy,:)=120;
blueplane(:,isocenter_imx)=120;

M(:, :, 3)=blueplane;


blueplane=M(:, :, 2);
blueplane(isocenter_imy,:)=120;
blueplane(:,isocenter_imx)=120;

M(:, :, 2)=blueplane;

blueplane=M(:, :, 1);
blueplane(isocenter_imy,:)=120;
blueplane(:,isocenter_imx)=120;

M(:, :, 1)=blueplane;


figure, imshow(M);



%imshowpair(BW1,BW2,'montage')
%title('Sobel Filter                                   Canny Filter');

%im=image(B)
%imshow(im)
%figure, imshow(M);
%a=image(B)
%imshow(a)
%im = image(B)
%colormap(gray(256))
%imshow(im)
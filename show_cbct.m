A = importdata('cbct_slice.txt');
my_Size=size(A);
orientation=384;
%for cbct and ct both
%isocx=-15.86;
%isocy=-129.49;

isocx=-204.86;
isocy=-205.49;


%for cbct
x_origin=0;
y_origin=0;


pixel_width=1.172;
%pixel_height=1.172*(384/512);
trans_facx=189/pixel_width;
trans_facy=79.52/pixel_width;

central_x=(orientation)/2 ;
central_y=(orientation)/2 ;

central_x=(orientation)/2 + trans_facx ;
central_y=(orientation)/2 + trans_facy ;


isocenter_imx=floor(central_x+(isocx/pixel_width));
isocenter_imy=floor(central_y+(isocy/pixel_height));

%isocenter_imx=floor(isocx/pixel_width);
%isocenter_imy=floor(isocy/pixel_height);


for n = 1:my_Size(2)
    if A(n) > 254
        A(n) = 254; 
    end
end
%for ct
B=reshape(A,orientation,orientation);
B = imtranslate(B,[-trans_facy, -trans_facx]);
%B = imresize(B, [1 1]); 
%B=imtransform(B,[trans_facx, trans_facy]);
%figure, imshow(B);
%y,x
B(isocenter_imy,:)=254;
B(:,isocenter_imx)=254;

%B(steps_in_x,:)=254;
%B(:,steps_in_y)=254;


%for ct
%B=reshape(A,384,384);


%B(4,:)=254;

M=mat2gray(B);
figure, imshow(M);

B=M;
%m = makehgtform('xrotate',pi/2,'yrotate',pi/2);
%M = makehgtform('scale',[sx,sy,sz]);
%M = makehgtform('translate',[tx ty tz])
sx = sqrt(0.999^2);
sy = sqrt(0.999^2);

%m = makehgtform('translate',[189.11 75.59 1],'scale',[sx sy 1]);

trans_mat=[1 0 0; 0 1 0; 189.11 75.59 1];
scale_mat=[sx 0 0; 0 sy 0; 0 0 1];

tform_trans = affine2d(trans_mat);
C = imwarp(M, tform_trans);
%imshow(C)
trans_scale = affine2d(scale_mat);
D = imwarp(C, trans_scale);
figure,imshow(D);
%BW1 = edge(M,'sobel');
%nnz(BW1)
 %figure, imshow(B);
 %sfigure, imshow(D);


%{
BW1 = edge(M,'sobel');
BW2 = edge(M,'canny');
figure;
imshow(BW1);

M = cat(3, M, M, M);
blueplane=M(:, :, 3);
blueplane(1,:)=120;
blueplane(2,:)=120;

M(:, :, 3)=blueplane;



%}
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
   M S B A M L   `   `   `  ����I  EObjectMovingUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:ObjectMovingUIObjectMovingUI  5   	       ObjectMovingUI.Setting   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:ObjectMovingUI     5   	     � Title$  Setting��5   	   $	��200��6   $	��400��6&   .��5	      �  � RowDefinitions 5
   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� -   5        $��isOnline��5   (   $��20, 0, 0, 0q�6   $��Trực tuyến��68   $��0��6M   $��0��6Z   $��Center=�6j   $
��Left=�6�   ��.   J� IsHitTestVisible . 6�   	� 5   
   -   5        $��FTPType��5   +   $��20, 0, 0, 0q�6    	� 	GroupName$
 type��6:   $	��FTP��6K   $��1��6Y   $��0��6f   $��Center=�6v   $
��Left=�6�   $y�	Collapsed=�6�   	� 5   
   -   5        $��
SocketType��5      $
 type��6(   $��Socket��69   $��1��6J   $��1��6W   $��Center=�6g   $
��Left=�6�   $y�	Collapsed=�6�   �� 5   
   -   5        $��Sp��5       �� Orientation$ 
Horizontal=�6   $��2��68   $��1��6E   $��Center=�6U   $
��Left=�6p   $y�	Collapsed=�6�   .+�5      �� $��IP��6   $��Center=�6!   �� 5      -   5        $��IPAdress��5      $	��100��6&   $��	localhost��62   $��Center=�6C   �� 5      $
��Port��6   $��Center=�6#   �� 5      -   5        $
��Port��5      $��40��6"   $
��8080��6-   $��Center=�69   5      �� 5   
   $ 
Horizontal=�6   $��3��6.   $��1��6;   $��Center=�6K   $
��Left=�6f   .+�5      �� -   5         ��Name$
 save��5      $
��Lưu��6#   $��Right=�61   $��27��6M   $��Center=�6Y   $��55��6t    �� IsCancel . $
��Bold"�6�   
�������6�   �� 5       -   5        $ cancel��5       $��Hủy��6%   $��Right=�63   $��27��6O   $��Center=�6[   $��55��6v    . 
�������$��10, 0, 10, 0q�6�   5!      5"      5#      
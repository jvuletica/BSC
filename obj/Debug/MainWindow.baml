   M S B A M L   `   `   `  ����  BSC    BSC.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  BSC - Bad SSH Client��5   	   $	��350��6&   $	��525��63    � 
ResizeMode$ CanMinimize=�5   	   .��5      � -   5         �Name$
 grid��5       � RowDefinitions 5   
   �� 5      $��50 �6   6*   �� 5	      6   �� 5
      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6    � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!   �� 5      6!   �� 5      6!   .+�5   
   �� -   5         ��Name$ 
connectBtn��5      $��0��6%   $��3��62   .��6F   	Connect6O   �� 5   
   -   5        $ helpBtn��5      $��0��6"   $��3��6/   .��6P   Help � ToolTip 5      �� 5      <:- drop file into console to upload it to current directory�� 5      CA- use download command to download it, e.g. "download ./file.txt"5      5      �� 5    
   -   5         ��Name$ 	hostLabel��5       $��1��6#   $��4��60   $��Center=�6D   $��Center=�6a   .��6|   Host6�   �� 5!   
   -   5         ��Name$ 
hostString��5!      $��2��6&   $��4��63   $��150, 0, 150, 25q�6G   6�   �� 5"   
   -   5        $ 	userLabel��5"      $��3��6#   $��4��60   $��Center=�6D   $��Center=�6a   .��6|   
UserName6�   �� 5#   
   -   5        $ 
userString��5#      $��4��6&   $��4��63   $��150, 0, 150, 25q�6G   6�   �� 5$   
   -   5        $ passwordLabel��5$      $��5��6'   $��4��64   $��Center=�6H   $��Center=�6e   .��6�   
Password6�   �� 5%   
   -	   5        $ passwordString��5%      $��6��6*   $��4��67   $��150, 0, 150, 25q�6K   6�   �� 5&   
   -
   5        $ console��5&      $��1��6#   $��6��60   $��4��6A   	 J� 	AllowDrop	 . 6�   5'      5(      
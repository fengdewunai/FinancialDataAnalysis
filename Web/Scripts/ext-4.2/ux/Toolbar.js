/**************************************************************************************************
Copyright (C) Beijing Beida Software Engineering Development Co., Ltd. & Author
All rights reserved.
Author:Kenshin Cui
Date:3/30/2013 5:53:52 PM
Description:基于Ext4.x的Model扩展组件
*************************************************************************************************/
Ext.define('BeidaSoft.toolbar.Toolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.bdtoolbar',
    alternateClassName: 'BeidaSoft.Toolbar',
    xtype:'bdtoolbar',
    initComponent: function () {
        this.callParent(arguments);
    }
});
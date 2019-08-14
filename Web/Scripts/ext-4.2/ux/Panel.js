/**************************************************************************************************
Copyright (C) Beijing Beida Software Engineering Development Co., Ltd. & Author
All rights reserved.
Author:Kenshin Cui
Date:3/26/2013 8:23:13 PM
Description:基于Ext4.x的Panel扩展组件
*************************************************************************************************/
Ext.define('BeidaSoft.panel.Panel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.bdpanel',
    alternateClassName: 'BeidaSoft.Panel',
    initComponent: function () {
        this.callParent(arguments);
    },
    onDestory: function () {
        this.callParent(arguments);
        
    }
});
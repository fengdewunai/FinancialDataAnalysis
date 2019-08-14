/**************************************************************************************************
Copyright (C) Beijing Beida Software Engineering Development Co., Ltd. & Author
All rights reserved.
Author:Kenshin Cui
Date:3/30/2013 3:49:19 PM
Description:基于Ext4.x的Model扩展组件
*************************************************************************************************/
Ext.define('BeidaSoft.data.Store', {
    extend: 'Ext.data.Store',
    alias: 'widget.bdstore',
    initComponent: function () {
        this.callParent(arguments);
    }
});
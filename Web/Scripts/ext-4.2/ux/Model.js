/**************************************************************************************************
Copyright (C) Beijing Beida Software Engineering Development Co., Ltd. & Author
All rights reserved.
Author:Kenshin Cui
Date:3/30/2013 3:33:19 PM
Description:基于Ext4.x的Model扩展组件
*************************************************************************************************/
Ext.define('BeidaSoft.data.Model', {
    extend: 'Ext.data.Model',
    alias: 'widget.bdmodel',
    initComponent: function () {
        this.callParent(arguments);
    }
});
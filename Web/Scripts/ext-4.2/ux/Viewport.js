/**************************************************************************************************
Copyright (C) Beijing Beida Software Engineering Development Co., Ltd. & Author
All rights reserved.
Author:Kenshin Cui
Date:3/26/2013 9:19:23 AM
Description:基于Ext4.x的Viewport扩展
*************************************************************************************************/
Ext.namespace("BeidaSoft");
Ext.define('BeidaSoft.container.Viewport', {
    extend: 'Ext.container.Viewport',
    alias: 'widget.bdviewport',
    alternateClassName: 'BeidaSoft.Viewport',
    layout: 'border',
    north: null,
    west: null,
    south: null,
    east: null,
    center: null,
    initComponent: function () {
        var me = this;
        var regions = [];
        var pnCenter = me.center;
        if (pnCenter == null) {
            pnCenter = { xtype: 'panel', region: 'center' };
        } else if (!pnCenter.hasOwnProperty('region')) {
            pnCenter['region'] = 'center';
        }
        regions.push(pnCenter);
        var pnNorth = me.north;
        if (pnNorth != null) {
            if (!pnNorth.hasOwnProperty('region')) {
                pnNorth['region'] = 'north';
            }
            regions.push(pnNorth);
        }
        var pnEast = me.east;
        if (pnEast != null) {
            if (!pnEast.hasOwnProperty('region')) {
                pnEast['region'] = 'east';
            }
            regions.push(pnEast);
        }
        var pnSouth = me.south;
        if (pnSouth != null) {
            if (!pnSouth.hasOwnProperty('region')) {
                pnSouth['region'] = 'south';
            }
            regions.push(pnSouth);
        }
        var pnWest = me.west;
        if (pnWest != null) {
            if (!pnWest.hasOwnProperty('region')) {
                pnWest['region'] = 'west';
            }
            regions.push(pnWest);
        }

        me.items = regions;

        this.callParent(arguments);
    }
});

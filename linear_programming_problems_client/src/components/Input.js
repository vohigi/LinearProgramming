import React from 'react';
import PropTypes from 'prop-types';

const Input = ({id,name,value,onChange,...rest}) => {
    return (
        <input type="text" id={id} name={name} {...rest}
               value={value} onChange={onChange}/>
    );
};

Input.propTypes = {};
Input.defaultProps = {};

export default Input;
